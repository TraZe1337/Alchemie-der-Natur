using UnityEngine;

namespace RedstoneinventeGameStudio
{
    /// <summary>
    /// Handles actions that can be performed by items in the hotbar
    /// </summary>
    public static class HotBarActions
    {
        /// <summary>
        /// Executes the specified action method with the given item data
        /// </summary>
        /// <param name="actionMethodName">Name of the action method to execute</param>
        /// <param name="itemName">Name of the item being used</param>
        public static void ExecuteAction(string actionMethodName, string itemName)
        {
            Debug.Log($"Using item: {itemName}");

            switch (actionMethodName)
            {
                case "HealPlayer":
                    HealPlayer(50);
                    break;
                case "RestoreMana":
                    RestoreMana(30);
                    break;
                default:
                    Debug.LogWarning($"No method found for action: {actionMethodName}");
                    break;
            }
        }

        /// <summary>
        /// Heals the player by the specified amount
        /// </summary>
        /// <param name="amount">Amount to heal</param>
        private static void HealPlayer(int amount)
        {
            Debug.Log($"Player healed by {amount} HP.");
            // Implementation for healing player would go here
        }

        /// <summary>
        /// Restores player mana by the specified amount
        /// </summary>
        /// <param name="amount">Amount of mana to restore</param>
        private static void RestoreMana(int amount)
        {
            Debug.Log($"Player mana restored by {amount}.");
            // Implementation for restoring mana would go here
        }
    }
}
