using UnityEngine;
using RedstoneinventeGameStudio;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private CardManager[] inventoryCardManagers; // Corresponding CardManagers
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private int FindFreeSlot()
    {
        for (int i = 0; i < inventoryCardManagers.Length; i++)
        {
            if (inventoryCardManagers[i] != null && !inventoryCardManagers[i].isOccupied)
            {
                Debug.Log($"Found free slot at index {i}");
                return i;
            }
        }
        Debug.Log("No free slot found in the inventory.");
        return -1;
    }
    
    public void AddItemToInventory(InventoryItemData itemData)
    {
        int freeInventoryFieldIndex = FindFreeSlot();
        if (freeInventoryFieldIndex > 0)
        {
            CardManager cardManager = inventoryCardManagers[freeInventoryFieldIndex];
            if (cardManager == null)
            {
                Debug.LogError($"Inventory slot {freeInventoryFieldIndex} has no CardManager assigned!");
                return;
            }

            cardManager.SetItem(itemData); // Place the item in the free slot
            Debug.Log($"Added {itemData.name} to inventory at slot {freeInventoryFieldIndex}.");
        }
        else
        {
            Debug.Log("Inventory is full, cannot add item.");
            return;
        }
    }
}
