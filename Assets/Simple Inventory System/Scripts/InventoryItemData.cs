using UnityEngine;
using System; // Required for Action

namespace RedstoneinventeGameStudio
{
    [CreateAssetMenu(fileName = "Inventory Item", menuName = "Inventory Item")]
    public class InventoryItemData : ScriptableObject
    {
        public string itemName;
        public string itemDescription;
        public string itemTooltip;
        public Sprite itemIcon;

        public string actionMethodName; // Name of the method to execute
        public PlantSO plantData; // Reference to the PlantSO for plant items
    }
}