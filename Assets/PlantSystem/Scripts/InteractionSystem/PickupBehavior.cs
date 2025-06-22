using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class PickupBehavior : MonoBehaviour, IInteractable
{
    [Tooltip("InteractionType that triggers pick up / drop (default: Primary)")]
    [SerializeField] private InteractionType interactionType = InteractionType.Primary;

    [Tooltip("Distance from player to drop the object")]
    [SerializeField] private float dropDistance = 1f;

    [Tooltip("Layers considered as ground for drop placement")]
    [SerializeField] private LayerMask groundLayer;

    [Tooltip("Maximum height above drop point to start ground check")]
    [SerializeField] private float groundCheckHeight = 2f;
    [Tooltip("Point where the object will be held (e.g., player's hand)")]
    [SerializeField] private UsageType type;
    public UsageType UsageType => type;
    [SerializeField] private Transform childTransform;
    [SerializeField] private Rig rigComponent;
    private Rigidbody rb;
    private bool isHeld;
    public Transform holdPoint;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void Interact(InteractionType type, PlayerInteraction interactor) {
        Debug.Log($"Interact called on {gameObject.name} with type {type} by {interactor.gameObject.name}");
        if (type != interactionType) return;
        Debug.Log($"Interaction type matched: {type}");
        if (!isHeld) PickUp(interactor);
        else DropNextToPlayer(interactor);
    }

    private void PickUp(PlayerInteraction interactor) {
        Debug.Log($"Picking up {gameObject.name} by {interactor.gameObject.name}");

        transform.SetParent(holdPoint);
        switch (type) {
            case UsageType.None:
                if (rigComponent != null) {
                    rigComponent.weight = 1f;
                }
                break;
        }

        transform.localPosition = Vector3.zero;
        //transform.localRotation = Quaternion.identity * Quaternion.Euler(31.136f, 180f, 0.124f);

        rb.isKinematic = true;
        isHeld = true;
    }

    private void DropNextToPlayer(PlayerInteraction interactor) {
        Debug.Log($"Dropping {gameObject.name} by {interactor.gameObject.name}");
        
        transform.SetParent(null);
        switch (type) {
            case UsageType.None:
                if (rigComponent != null) {
                    rigComponent.weight = 0f;
                }
                break;
        }


        float colliderHeight = 0f;
        BoxCollider boxCollider = null;
        try
        {
            boxCollider = GetComponent<BoxCollider>();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"No BoxCollider found on {gameObject.name}. Error: {e.Message}");
            colliderHeight = transform.position.y - childTransform.localPosition.y;
        }
        colliderHeight = boxCollider.size.y;
        Debug.Log($"Y Offset: {colliderHeight}");

        
        // Calculate drop position in front of player
        Vector3 origin = interactor.transform.position;
        Vector3 forward = interactor.transform.forward;
        Vector3 start = origin + forward * dropDistance + Vector3.up * groundCheckHeight;

        // Raycast down to find ground
        if (Physics.Raycast(start, Vector3.down, out RaycastHit hit, groundCheckHeight * 2f, groundLayer)) {
            transform.position = hit.point + Vector3.up * colliderHeight;
            //parentTransform.position = hit.point;
            Debug.Log($"EXACTELY Dropped {gameObject.name} at {hit.point}");
        } else {
            transform.position = origin + Vector3.up * colliderHeight + forward * dropDistance;
            Debug.Log($"Dropped {gameObject.name} at {hit.point}");
        }

        rb.isKinematic = false;
        isHeld = false;

        // Optional: add forward toss
        //rb.AddForce(forward * 2f, ForceMode.VelocityChange);
    }
}
