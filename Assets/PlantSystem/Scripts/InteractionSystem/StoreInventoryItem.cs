using UnityEngine;

public class StoreInventoryItem : MonoBehaviour, IInteractable
{
    [Tooltip("InteractionType that adds item to inventory (default: Secondary)")]
    [SerializeField] private InteractionType interactionType = InteractionType.Secondary;

    public void Interact(InteractionType type, PlayerInteraction interactor) {
        if (type != interactionType) return;
        AddToInventory();
    }

    private void AddToInventory() {
        // Dummy implementation for inventory addition
        Debug.Log($"Added {gameObject.name} to inventory.");
        // TODO: Integrate with inventory system
    }
}
