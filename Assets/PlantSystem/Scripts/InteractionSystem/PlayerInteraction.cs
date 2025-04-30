using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Tooltip("Camera used for aiming interactions (e.g., the third-person camera)")]
    public Camera playerCamera;
    public GameObject playerChest;

    [Tooltip("Maximum distance for raycast interactions")]
    public float interactRange = 4f;

    [Tooltip("Which layers contain interactable objects")]
    public LayerMask interactLayer;

    [Tooltip("Transform where picked-up items will attach (e.g., hand)")]
    public Transform holdPoint;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Y)) {
            HandleInteraction(InteractionType.Primary);
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            HandleInteraction(InteractionType.Secondary);
        }
    }

    private void HandleInteraction(InteractionType type) {
    Ray ray = new Ray(playerChest.transform.position, playerCamera.transform.forward);
    Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.red, 1f);
    
    
    if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer)) {
        var interactable = hit.collider.GetComponentInParent<IInteractable>();
        Debug.Log($"Hit: {hit.collider.name}, Interactable: {interactable}");
        Debug.Log("Hit GameObject: " + hit.collider.gameObject.name);
        if (interactable != null) {
            interactable.Interact(type, this);
        }
    }
    }
}
