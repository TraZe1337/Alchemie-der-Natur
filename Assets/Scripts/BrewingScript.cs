using UnityEngine;
using System.Collections.Generic;
using RedstoneinventeGameStudio;
using System;
using TMPro;
using System.Collections;

public class BrewingScript : MonoBehaviour
{
    [SerializeField] private CardManager[] cauldronSlots; // Array of item slots (3 slots).
    [SerializeField] private List<Recipe> recipes; // List of valid recipes.
    [SerializeField] private CardManager[] inventorySlots; // Array of inventory slots.
    [SerializeField] private GameObject popupText; // Reference to the TextMeshPro UI text for feedback.
    [SerializeField] private float popupDuration = 2f; // Duration for which the popup text is visible.

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
        if (cauldronSlots.Length != 3)
        {
            Debug.LogError("Brewing requires exactly 3 item slots.");
        }

        // Ensure the popup text is initially hidden.
        if (popupText != null)
        {
            popupText.gameObject.SetActive(false); // Hide the popup text at the start.
        }
        else
        {
            Debug.LogError("Popup text reference is not assigned.");
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
        InventoryItemData[] itemsInSlots = new InventoryItemData[cauldronSlots.Length];
        for (int i = 0; i < cauldronSlots.Length; i++)
        {
            if (cauldronSlots[i].itemData == null)
            {
                Debug.Log("One or more slots are empty.");

                popupText.gameObject.SetActive(true); // Ensure the popup is visible.
                popupText.GetComponentInChildren<TextMeshProUGUI>().text = "One or more slots are empty.";
                StartCoroutine(HidePopupAfterDelay()); // Start coroutine to hide the popup.
                return;
            }
            itemsInSlots[i] = cauldronSlots[i].itemData;
        }

        // Check if the items match any recipe.
        foreach (var recipe in recipes)
        {
            if (IsMatchingRecipe(itemsInSlots, recipe.requiredItems))
            {
                Debug.Log($"Potion brewed: {recipe.resultingPotion.name}");

                popupText.gameObject.SetActive(true); // Ensure the popup is visible.
                popupText.GetComponentInChildren<TextMeshProUGUI>().text = $"Potion brewed: {recipe.resultingPotion.name}"; // Show success message.
                StartCoroutine(HidePopupAfterDelay()); // Start coroutine to hide the popup.
                ClearSlots();
                AddItemToInventory(recipe.resultingPotion); // Add the resulting potion to the inventory.
                // Optionally, you can also play a brewing sound or show a visual effect here.
                return;
            }
        }

        Debug.Log("No matching recipe found.");
        popupText.GetComponentInChildren<TextMeshProUGUI>().text = "No matching recipe found."; // Show failure message.
        popupText.gameObject.SetActive(true); // Ensure the popup is visible.
        StartCoroutine(HidePopupAfterDelay()); // Start coroutine to hide the popup.
    }

    // Coroutine to hide the popup text after a delay.
    private IEnumerator HidePopupAfterDelay()
    {
        yield return new WaitForSeconds(popupDuration);
        popupText.gameObject.SetActive(false); // Hide the popup text.
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

    public void ClearSlots()
    {
        // Clear all item slots after brewing.
        foreach (var slot in cauldronSlots)
        {
            slot.UnSetItem();
        }
    }
    public void ClearSlotsToInventory()
    {
        // Clear all item slots after brewing.
        foreach (var slot in cauldronSlots)
        {
            if (slot.itemData == null) continue; // Skip empty slots.
            AddItemToInventory(slot.itemData); // Add the item back to the inventory.
            slot.UnSetItem();
        }

    }

    private void AddItemToInventory(InventoryItemData item)
    {
        foreach (var slot in inventorySlots)
        {
            if (!slot.isOccupied) // Check if the slot is free.
            {
                slot.SetItem(item); // Place the potion in the free slot.
                Debug.Log($"Added {item.name} to inventory.");

                return;
            }
        }

        Debug.Log("No free slots available in the inventory.");
    }
}
