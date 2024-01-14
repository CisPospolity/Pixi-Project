using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerScript))]
public class PlayerInteractables : MonoBehaviour
{

    private PlayerScript playerScript;
    private PlayerInputSystem playerInputSystem;

    //List of Interactable objects nearby
    [SerializeField]
    private List<IInteractable> interactables = new List<IInteractable>();

    //Object of UI to show above
    [SerializeField]
    private GameObject interactUI;

    private void Awake()
    {
        playerScript = GetComponent<PlayerScript>();
        playerInputSystem = GetComponent<PlayerInputSystem>();

        playerInputSystem.onInteract += Interact;
    }

    public void AddInteractable(IInteractable interactable)
    {
        interactables.Add(interactable);
    }

    public void RemoveInteractable(IInteractable interactable)
    {
        interactables.Remove(interactable);
    }

    private void Update()
    {
        SetUI();
    }

    private IInteractable GetClosestInteractable()
    {
        if (interactables.Count > 0)
        {
            IInteractable closest = null;
            float closestDistance = float.MaxValue;
            foreach (IInteractable interactable in interactables)
            {
                if (!interactable.CanInteract()) continue;
                if (Vector3.Distance(gameObject.transform.position, interactable.GetGameObject().transform.position) < closestDistance)
                {
                    closest = interactable;
                    closestDistance = Vector3.Distance(gameObject.transform.position, interactable.GetGameObject().transform.position);
                }
            }
            return closest;
        }
        return null;
    }

    private void Interact()
    {
        IInteractable closest = GetClosestInteractable();
        if (closest == null) return;

        closest.Interact(playerScript);
    }

    //Set UI Element above Interactable
    private void SetUI()
    {
        if (interactUI == null) return;
        IInteractable closest = GetClosestInteractable();
        if (closest == null)
        {
            interactUI.SetActive(false);
            return;
        }

        interactUI.SetActive(true);
        interactUI.transform.position = closest.GetPositionForUI();
        interactUI.transform.LookAt(Camera.main.transform);
    }

    public void SetInteractUIReference(GameObject interactUIReference)
    {
        interactUI = interactUIReference;
    }
}