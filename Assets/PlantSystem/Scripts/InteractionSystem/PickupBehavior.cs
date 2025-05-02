using UnityEngine;

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
    [SerializeField] private Transform parentTransform;
    [SerializeField] private Transform childTransform;


    private Rigidbody rb;
    private bool isHeld;
    private Transform holdPoint;

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
        holdPoint = interactor.holdPoint;
        parentTransform.SetParent(holdPoint);

        parentTransform.localPosition = Vector3.zero;
        parentTransform.localRotation = Quaternion.identity;

        rb.isKinematic = true;
        isHeld = true;
    }

    private void DropNextToPlayer(PlayerInteraction interactor) {
        Debug.Log($"Dropping {gameObject.name} by {interactor.gameObject.name}");
        parentTransform.SetParent(null);

        float yOffset = parentTransform.position.y - childTransform.localPosition.y;
        Debug.Log($"Y Offset: {yOffset}");
        // Calculate drop position in front of player
        Vector3 origin = interactor.transform.position;
        Vector3 forward = interactor.transform.forward;
        Vector3 start = origin + forward * dropDistance + Vector3.up * groundCheckHeight;

        // Raycast down to find ground
        if (Physics.Raycast(start, Vector3.down, out RaycastHit hit, groundCheckHeight * 2f, groundLayer)) {
            parentTransform.position = hit.point + Vector3.up * yOffset;
            //parentTransform.position = hit.point;
            Debug.Log($"EXACTELY Dropped {gameObject.name} at {hit.point}");
        } else {
            parentTransform.position = origin + Vector3.up * yOffset + forward * dropDistance;
            Debug.Log($"Dropped {gameObject.name} at {hit.point}");
        }

        rb.isKinematic = false;
        isHeld = false;

        // Optional: add forward toss
        //rb.AddForce(forward * 2f, ForceMode.VelocityChange);
    }
}
