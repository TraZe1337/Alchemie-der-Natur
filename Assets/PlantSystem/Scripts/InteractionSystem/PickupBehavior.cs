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

    private Rigidbody rb;
    private bool isHeld;
    private Transform holdPoint;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void Interact(InteractionType type, PlayerInteraction interactor) {
        if (type != interactionType) return;

        if (!isHeld) PickUp(interactor);
        else DropNextToPlayer(interactor);
    }

    private void PickUp(PlayerInteraction interactor) {
        holdPoint = interactor.holdPoint;
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        rb.isKinematic = true;
        isHeld = true;
    }

    private void DropNextToPlayer(PlayerInteraction interactor) {
        transform.SetParent(null);

        // Calculate drop position in front of player
        Vector3 origin = interactor.transform.position;
        Vector3 forward = interactor.transform.forward;
        Vector3 start = origin + forward * dropDistance + Vector3.up * groundCheckHeight;

        // Raycast down to find ground
        if (Physics.Raycast(start, Vector3.down, out RaycastHit hit, groundCheckHeight * 2f, groundLayer)) {
            transform.position = hit.point;
        } else {
            transform.position = origin + forward * dropDistance;
        }

        rb.isKinematic = false;
        isHeld = false;

        // Optional: add forward toss
        rb.AddForce(forward * 2f, ForceMode.VelocityChange);
    }
}
