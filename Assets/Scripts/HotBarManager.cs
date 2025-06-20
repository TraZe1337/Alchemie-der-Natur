using UnityEngine;
using UnityEngine.UI;
using RedstoneinventeGameStudio;

public class HotBarManager : MonoBehaviour
{

    [Header("Hotbar Slots")]
    [SerializeField] private CardManager[] hotbarCardManagers = new CardManager[5]; // Corresponding CardManagers

    [Header("Debug")]
    [SerializeField] private bool showDebugMessages = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize hotbar card managers
        for (int i = 0; i < hotbarCardManagers.Length; i++)
        {
            if (hotbarCardManagers[i] != null)
            {
                if (showDebugMessages)
                {
                    Debug.Log($"Hotbar slot {i} is initialized");
                }
            }
            else
            {
                Debug.LogWarning($"Hotbar slot {i} has no CardManager assigned!");
            }
        }
    }

    /// <summary>
    /// Uses the item in the specified hotbar slot
    /// </summary>
    /// <param name="slotIndex">The index of the hotbar slot (0-4)</param>
    public void UseHotbarItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < hotbarCardManagers.Length && hotbarCardManagers[slotIndex] != null)
        {
            CardManager cardManager = hotbarCardManagers[slotIndex];

            // If this slot has an item, use it
            if (cardManager.isOccupied && cardManager.itemData != null)
            {
                if (showDebugMessages)
                {
                    Debug.Log($"Using hotbar slot {slotIndex}");
                }

                // Forward call to the CardManager's UseItem method
                cardManager.UseItem();
            }
            else
            {
                if (showDebugMessages)
                {
                    Debug.Log($"Hotbar slot {slotIndex} is empty or has no item assigned");
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Hotkey support using only the number keys above the letters (Alpha keys)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseHotbarItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseHotbarItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseHotbarItem(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UseHotbarItem(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            UseHotbarItem(4);
        }
    }
}
