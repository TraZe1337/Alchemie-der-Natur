using UnityEngine;
using RedstoneinventeGameStudio;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private CardManager[] inventoryCardManagers = new CardManager[25]; // Corresponding CardManagers
    [SerializeField] private InventoryItemData[] inventoryItems = new InventoryItemData[25]; // Array to hold item data for each slot
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
