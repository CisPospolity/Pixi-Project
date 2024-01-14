using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputSystem))]
public class CharacterMovement : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInputSystem playerInputSystem;

    #region Movement
    private Vector2 movementVector;
    private Vector2 rotation;
    [SerializeField]
    private float controllerDeadzone = 0.1f;
    [SerializeField]
    private float speed;
    private SpeedModifierManager speedModifierManager = new SpeedModifierManager();
    float speedMultiplier;
    public float ActualSpeed => speed * speedModifierManager.CurrentSpeedMultiplier;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float gravity = 24f;
    [SerializeField]
    private Vector3 playerVelocity = Vector3.zero;
    [SerializeField]
    private float groundCheckLength = 1f;

    private bool isDashing = false;
    #endregion

    public Vector2 GetMovementVector()
    {
        return movementVector;
    }

    public Vector3 GetRelativeMovementVector()
    {
        movementVector = movementVector.normalized;
        Vector3 forwardDir = Camera.main.transform.forward;
        forwardDir.y = 0;
        forwardDir = forwardDir.normalized;
        forwardDir = forwardDir * movementVector.y;
        Vector3 rightDir = Camera.main.transform.right;
        rightDir.y = 0;
        rightDir = rightDir.normalized;
        rightDir = rightDir * movementVector.x;
        Vector3 moveDir = (forwardDir + rightDir);
        return moveDir;
    }

    public void SetDashBool(bool isDashing)
    {
        this.isDashing = isDashing;
    }

    public void ResetHeightVelocity()
    {
        playerVelocity.y = 0;
    }

    public SpeedModifierManager GetSpeedModifierManager()
    {
        return speedModifierManager;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), "Actual Speed: " + ActualSpeed);
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInputSystem = GetComponent<PlayerInputSystem>();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerInputSystem.onMovement += MovementHandler;
        playerInputSystem.onJump += Jump;
        playerInputSystem.onLookRotation += RotationHandler;

        speedMultiplier = speedModifierManager.CurrentSpeedMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        //Gravity
        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

        speedMultiplier = speedModifierManager.CurrentSpeedMultiplier;

        PlayerMovement();
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        if(playerInputSystem.IsGamepad())
        {
            if (Mathf.Abs(rotation.x) > controllerDeadzone || Mathf.Abs(rotation.y) > controllerDeadzone) {
                Vector3 camForwardDir = Camera.main.transform.forward;
                camForwardDir.y = 0;
                Vector3 camRightDir = Camera.main.transform.right;
                camRightDir.y = 0;
                Vector3 playerDir = camRightDir * rotation.x + camForwardDir * rotation.y;
                if (playerDir.sqrMagnitude > 0)
                {
                    Quaternion newRot = Quaternion.LookRotation(playerDir, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, 1000f * Time.deltaTime);
                }
            }
        } else
        {
            Ray ray = Camera.main.ScreenPointToRay(rotation);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayDistance;
            if(groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                point.y = transform.position.y;
                transform.LookAt(point);
            }
        }
    }


    private void PlayerMovement()
    {
        if (isDashing) return;
        //WSAD Movement
        movementVector = movementVector.normalized;
        Vector3 forwardDir = Camera.main.transform.forward;
        forwardDir.y = 0;
        forwardDir = forwardDir.normalized;
        forwardDir = forwardDir * movementVector.y;
        Vector3 rightDir = Camera.main.transform.right;
        rightDir.y = 0;
        rightDir = rightDir.normalized;
        rightDir = rightDir * movementVector.x;
        Vector3 moveDir = (forwardDir + rightDir) * speed * speedMultiplier * Time.deltaTime;
        characterController.Move(moveDir);

        playerVelocity.y -= gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void MovementHandler(Vector2 dir)
    {
        movementVector = dir;
    }

    private void RotationHandler(Vector2 rot)
    {
        rotation = rot;
    }

    private void Jump()
    {
        if (!CheckGround()) return;

        playerVelocity.y += Mathf.Sqrt(jumpHeight * 2f * gravity);
    }

    private bool CheckGround()
    {
        return Physics.Raycast(new Ray(transform.position, Vector3.down), groundCheckLength);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckLength);
    }
}
