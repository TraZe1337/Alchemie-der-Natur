using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform holdPoint;
    public float interactRange = 3f;
    public LayerMask interactableMask;
    private IInteractable currentInteractable;
    private GameObject heldObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleDetection();

        if (Input.GetKeyDown(KeyCode.E)) // Primary interaction
        {
            InteractWithObject("Primary");
        }
        else if (Input.GetKeyDown(KeyCode.F)) // Secondary interaction
        {
            InteractWithObject("Secondary");
        }
    }

    void HandleDetection()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableMask))
        {
            currentInteractable = hit.collider.GetComponent<IInteractable>();
        }
        else
        {
            currentInteractable = null;
        }
    }

    void InteractWithObject(string interactionType)
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact(interactionType);

            if (interactionType == "Primary" && heldObject == null)
            {
                TryPickupObject();
            }
        }
    }

    void TryPickupObject()
    {
        if (currentInteractable is InteractableObject interactable && interactable.canBePickedUp)
        {
            heldObject = interactable.gameObject; 
            heldObject.transform.SetParent(holdPoint);
            heldObject.transform.localPosition = Vector3.zero;
            heldObject.transform.localRotation = Quaternion.identity;

            var rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = true;
        }
    }

    public void DropHeldObject()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null);
            var rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = false;
            heldObject = null;
        }
    }
}
