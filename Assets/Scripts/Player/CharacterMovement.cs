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
    private float speed;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float gravity = 24f;
    [SerializeField]
    private Vector3 playerVelocity = Vector3.zero;
    #endregion
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
    }

    // Update is called once per frame
    void Update()
    {
        //Gravity
        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

        RotatePlayer();
    }

    private void RotatePlayer()
    {
        if(playerInputSystem.IsGamepad())
        {
            Vector3 camForwardDir = Camera.main.transform.forward;
            camForwardDir.y = 0;
            Vector3 camRightDir = Camera.main.transform.right;
            camRightDir.y = 0;
            Vector3 playerDir = camRightDir * rotation.x + camForwardDir * rotation.y;
            if(playerDir.sqrMagnitude > 0)
            {
                Quaternion newRot = Quaternion.LookRotation(playerDir, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, 1000f * Time.deltaTime);
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


    private void FixedUpdate()
    {

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
        Vector3 moveDir = (forwardDir + rightDir) * speed * Time.deltaTime;
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
        if (!characterController.isGrounded) return;

        playerVelocity.y += Mathf.Sqrt(jumpHeight * 2f * gravity);
    }
}
