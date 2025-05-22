using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Best Practices:
/// - Use a dedicated Layer (e.g. "Pickupable") for performance filtering in physics queries.
/// - Implement an IPickupable interface on items for flexibility over simple tags.
/// - Attach this PickupItems script to the Player (or Inventory Manager) game object.
/// - Place scripts under Assets/Scripts/Interactions or similar folder structure.
/// </summary>

public interface IPickupable
{
    void OnPickup(PlayerInventory inventory);
}

[RequireComponent(typeof(CharacterController))]
public class PickupItems : MonoBehaviour
{
    [Header("Pickup Settings")]
    [Tooltip("Input key to pick up items")]
    public KeyCode pickupKey = KeyCode.E;

    [Tooltip("Maximum distance to pick up items")]
    public float pickupRange = 2f;

    [Tooltip("Layer mask to filter pickupable items")]    
    public LayerMask pickupLayer;

    // Cache player's inventory component
    private PlayerInventory _inventory;
    // Cache transform for efficiency
    private Transform _cam;

    private void Awake()
    {
        _inventory = GetComponent<PlayerInventory>();
        _cam = Camera.main.transform;

        if (_inventory == null)
            Debug.LogError("PlayerInventory component missing on " + gameObject.name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            TryPickup();
        }
    }

    /// <summary>
    /// Casts a sphere from camera center to detect and pick up the first IPickupable item
    /// </summary>
    private void TryPickup()
    {
        Ray ray = new Ray(_cam.position, _cam.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.5f, out hit, pickupRange, pickupLayer, QueryTriggerInteraction.Collide))
        {
            IPickupable pickupable = hit.collider.GetComponent<IPickupable>();
            if (pickupable != null)
            {
                pickupable.OnPickup(_inventory);
            }
        }
    }
}

/// <summary>
/// Example inventory component to store picked items
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    private List<GameObject> _items = new List<GameObject>();

    public void AddItem(GameObject item)
    {
        _items.Add(item);
        // Customize: update UI, stats, etc.
        Debug.Log(item.name + " added to inventory.");
    }
}

/// <summary>
/// Example pickupable item script
/// </summary>
[RequireComponent(typeof(Collider))]
public class PickupableItem : MonoBehaviour, IPickupable
{
    private void Awake()
    {
        // Ensure collider is trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    public void OnPickup(PlayerInventory inventory)
    {
        inventory.AddItem(gameObject);
        // Disable or destroy after pickup
        gameObject.SetActive(false);
    }
}
