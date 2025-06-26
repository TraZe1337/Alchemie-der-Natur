using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Tooltip("Camera used for aiming interactions (e.g., the third-person camera)")]
    public Camera playerCamera;
    public GameObject playerChest;
    public InventoryManager inventoryManager;

    [Tooltip("Maximum distance for raycast interactions")]
    public float interactRange = 4f;
    public float usageOfHoldingObjectRange = 0.5f;

    [Tooltip("Which layers contain interactable objects")]
    public LayerMask interactLayer;

    [Tooltip("Transform where picked-up items will attach (e.g., hand)")]
    private bool isHoldingItem = false;
    private IInteractable currentInteractable = null;

    void Update() {
        // Use holding object interactions | Order is IMPORTANT because BUtton C is also used for info interactions
        if (isHoldingItem) {
            if (Input.GetKey(KeyCode.C)) {
                UseHoldingObject();
                return;
            }

            if (Input.GetKeyUp(KeyCode.C)) {
                Debug.Log("Stopped using holding object.");
                StopAllHoldingObjectEffects();
                return;
            }
        }
        // Pickup / drop and info interactions
        if (Input.GetKeyDown(KeyCode.X)) HandleInteraction(InteractionType.Primary); // Pickup / drop
        if (Input.GetKeyDown(KeyCode.C)) HandleInteraction(InteractionType.Secondary);
    }

    private void UseHoldingObject() {
        Debug.Log($"Using holding object: {currentInteractable}");
        Collider[] colliders = Physics.OverlapSphere(playerChest.transform.position, usageOfHoldingObjectRange);
        foreach (Collider collider in colliders) {
            if (collider.TryGetComponent(out IUsable usable)) {
                Debug.Log($"Using holding object on: {collider.gameObject.name}");
                PickupBehavior pb = currentInteractable as PickupBehavior;
                Debug.Log($"PickupBehavior: {pb}");
                switch (pb.UsageType) {
                    case UsageType.Watering:
                        usable.AddWater(pb.gameObject.GetComponent<WateringCan>().DispenseWater(Time.deltaTime));
                        break;
                    case UsageType.Fertilizer:
                        usable.AddNutrients(pb.gameObject.GetComponent<Fertilizer>().DispenseFertilizer(Time.deltaTime));
                        break;
                    case UsageType.Harvesting:
                        (int harvest, int seemen, PlantSO plantType) = usable.HarvestPotPlant();
                        Debug.Log($"Harvested {harvest} plants and {seemen} seemen from {collider.gameObject.name}");
                        // Adding plant items to inventory
                        if (harvest > 0 && plantType != null)
                        {
                            for (int i = 0; i < harvest; i++)
                            {
                                inventoryManager.AddItemToInventory(plantType.plantItemData);
                            }
                        }
                        // Adding seemen items to inventory
                        if (seemen > 0 && plantType != null)
                        {
                            for (int i = 0; i < seemen; i++)
                            {
                                inventoryManager.AddItemToInventory(plantType.seedItemData);
                            }
                        }
                        break;
                }
            }
        }
    }

    private void StopAllHoldingObjectEffects() {
        PickupBehavior pb = currentInteractable as PickupBehavior;
        switch (pb.UsageType) {
            case UsageType.Watering:
                Debug.Log($"Stopping watering effect on: {pb.gameObject.name}");
                pb.gameObject.GetComponent<WateringCan>().StopWatering();
                break;
            case UsageType.Fertilizer:
                pb.gameObject.GetComponent<Fertilizer>().StopFertilizing();
                break;
        }
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // draw a wire sphere at this object’s position
        Gizmos.DrawWireSphere(transform.position, usageOfHoldingObjectRange);
    }
    private void HandleInteraction(InteractionType type)
    {
        if (type == InteractionType.Primary && isHoldingItem)
        {
            Debug.Log($"Already holding an item, dropping it instead of interacting.");
            currentInteractable.Interact(type, this);
            isHoldingItem = false;
            currentInteractable = null;
            return;
        }
        
        Debug.Log($"Handling interaction of type {type}");
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.red, 1f);


        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            var interactable = hit.collider.GetComponentInParent<IInteractable>();
            Debug.Log($"Hit: {hit.collider.name}, Interactable: {interactable}");
            Debug.Log("Hit GameObject: " + hit.collider.gameObject.name);
            if (interactable != null)
            {
                interactable.Interact(type, this);

                if (type == InteractionType.Primary)
                {
                    currentInteractable = interactable;
                    isHoldingItem = true;
                }
            }
        }
    }
}