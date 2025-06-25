using UnityEngine;
using RedstoneinventeGameStudio;
using System;

public class PickupItemsGround : MonoBehaviour
{

    [Header("Inventar Settings")]
    [SerializeField] private CardManager[] inventorySlots;

    [Header("Pick-up Settings")]
    [SerializeField] private KeyCode pickupKey = KeyCode.Q;
    [SerializeField] private float pickupRange = 4f;
    [SerializeField] private LayerMask pickupLayer;    // im Inspector: GroundItems anhaken!
    [SerializeField] private Transform rayOrigin;      // z.B. deine Kamera

    private void OnValidate()
    {
        if (rayOrigin == null) rayOrigin = transform;
    }

    void Update()
    {
        // 1) Zeichne den Ray jede Frame (Scene-View, Gizmos an)
        Debug.DrawRay(rayOrigin.position, rayOrigin.forward * pickupRange, Color.red, 0.1f);

        if (Input.GetKeyDown(pickupKey))
        {
            // 2) LayerMask-Check
            int groundLayer = LayerMask.NameToLayer("GroundItems");
            bool maskContains = (pickupLayer & (1 << groundLayer)) != 0;
            Debug.Log($"[Debug] pickupLayer value: {Convert.ToString(pickupLayer.value, 2)}, " +
                      $"GroundItems bit: {Convert.ToString(1 << groundLayer, 2)}, " +
                      $"enthält GroundItems? {maskContains}");

            TryPickupItem();
        }
    }

    private void TryPickupItem()
    {
        Vector3 origin = rayOrigin.position;
        Vector3 dir    = rayOrigin.forward;

        // 1) Schieß auf alle Layer
        var hits = Physics.RaycastAll(origin, dir, pickupRange);
        foreach (var h in hits)
        {
            var groundItem = h.collider.GetComponent<GroundItem>();
            if (groundItem != null && groundItem.itemData != null)
            {
                Debug.Log($"[Pickup] Found GroundItem on {h.collider.name}");
                AddItemToInventory(groundItem.itemData);
                Destroy(h.collider.gameObject);
                return; // nur das erste aufheben
            }
        }

        Debug.Log("[Pickup] Kein aufhebbares GroundItem gefunden.");
    }
    private void AddItemToInventory(InventoryItemData item)
    {
        if (inventorySlots == null || inventorySlots.Length == 0)
        {
            Debug.LogError("[Pickup] inventorySlots ist leer!");
            return;
        }

        foreach (var slot in inventorySlots)
        {
            if (!slot.isOccupied)
            {
                slot.SetItem(item);
                Debug.Log($"[Pickup] Added {item.itemName} to inventory.");
                return;
            }
        }

        Debug.LogWarning("[Pickup] No free slots available in the inventory.");
    }
}
