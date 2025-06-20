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
                case "IncreaseDamageResist":
                    IncreaseDamageResist();
                    break;
                case "IncreasePerception":
                    IncreasePerception();
                    break;
                case "StealthPlayer":
                    StealthPlayer();
                    break;
                case "IncreaseElementalResist":
                    IncreaseElementalResist();
                    break;
                case "IncreaseDamagePlayer":
                    IncreaseDamagePlayer();
                    break;
                case "IncreaseFrostResistPlayer":
                    IncreaseFrostResistPlayer();
                    break;
                default:
                    Debug.LogWarning($"No method found for action: {actionMethodName}");
                    break;
            }
        }

        // Stub methods for new actions
        private static void IncreaseDamageResist()
        {
            Debug.Log("Player's damage resistance increased.");
            // Implementation goes here
        }

        private static void IncreasePerception()
        {
            Debug.Log("Player's perception increased.");
            // Implementation goes here
        }

        private static void StealthPlayer()
        {
            Debug.Log("Player is now in stealth mode.");
            // Implementation goes here
        }

        private static void IncreaseElementalResist()
        {
            Debug.Log("Player's elemental resistance increased.");
            // Implementation goes here
        }

        private static void IncreaseDamagePlayer()
        {
            Debug.Log("Player's damage increased.");
            // Implementation goes here
        }

        private static void IncreaseFrostResistPlayer()
        {
            Debug.Log("Player's frost resistance increased.");
            // Implementation goes here
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
    }
}
