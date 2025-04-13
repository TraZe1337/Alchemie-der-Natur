using UnityEngine;
using System.Collections.Generic;
using RedstoneinventeGameStudio;
using System;

public class BrewingScript : MonoBehaviour
{
    [SerializeField] private CardManager[] itemSlots; // Array of item slots (3 slots).
    [SerializeField] private List<Recipe> recipes; // List of valid recipes.
    [SerializeField] private CardManager[] inventorySlots; // Array of inventory slots.

    // Represents a recipe with required items and the resulting potion.
    [System.Serializable]
    public class Recipe
    {
        public InventoryItemData[] requiredItems; // Items required for the recipe.
        public InventoryItemData resultingPotion; // Resulting potion from the recipe.
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ensure there are exactly 3 item slots.
        if (itemSlots.Length != 3)
        {
            Debug.LogError("Brewing requires exactly 3 item slots.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // // Check for brewing input (e.g., a button press).
        // if (Input.GetKeyDown(KeyCode.B)) // Replace with your desired input.
        // {
        //     BrewPotion();
        // }
    }

    public void BrewPotion()
    {
        // Collect items from the slots.
        InventoryItemData[] itemsInSlots = new InventoryItemData[itemSlots.Length];
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].itemData == null)
            {
                Debug.Log("One or more slots are empty.");
                return;
            }
            itemsInSlots[i] = itemSlots[i].itemData;
        }

        // Check if the items match any recipe.
        foreach (var recipe in recipes)
        {
            if (IsMatchingRecipe(itemsInSlots, recipe.requiredItems))
            {
                Debug.Log($"Potion brewed: {recipe.resultingPotion.name}");
                ClearSlots();
                AddPotionToInventory(recipe.resultingPotion); // Add the resulting potion to the inventory.
                // Optionally, you can also play a brewing sound or show a visual effect here.
                return;
            }
        }

        Debug.Log("No matching recipe found.");
    }



    private bool IsMatchingRecipe(InventoryItemData[] itemsInSlots, InventoryItemData[] requiredItems)
    {
        // Ensure the number of items matches.
        if (itemsInSlots.Length != requiredItems.Length)
        {
            return false;
        }

        // Check if all required items are present in the slots.
        List<InventoryItemData> requiredItemsList = new List<InventoryItemData>(requiredItems);
        foreach (var item in itemsInSlots)
        {
            if (!requiredItemsList.Remove(item))
            {
                return false; // Item not found or duplicate.
            }
        }

        return true;
    }

    private void ClearSlots()
    {
        // Clear all item slots after brewing.
        foreach (var slot in itemSlots)
        {
            slot.UnSetItem();
        }
    }

    private void AddPotionToInventory(InventoryItemData resultingPotion)
    {
        foreach (var slot in inventorySlots)
        {
            if (!slot.isOccupied) // Check if the slot is free.
            {
                slot.SetItem(resultingPotion); // Place the potion in the free slot.
                Debug.Log($"Added {resultingPotion.name} to inventory.");
                return;
            }
        }

        Debug.Log("No free slots available in the inventory.");
    }
}
