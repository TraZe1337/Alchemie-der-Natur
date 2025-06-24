using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RedstoneinventeGameStudio
{
    /// <summary>
    /// Manages the behavior of an inventory card, including displaying item data,
    /// handling drag-and-drop functionality, and showing tooltips.
    /// </summary>
    public class CardManager : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
#nullable enable
        /// <summary>
        /// The data of the inventory item associated with this card.
        /// </summary>
        public InventoryItemData? itemData;

        /// <summary>
        /// Indicates whether this card currently holds an item.
        /// </summary>
        public bool isOccupied;
#nullable disable

        [SerializeField] bool useAsDrag; // Determines if this card is used as a drag placeholder.
        [SerializeField] GameObject emptyCard; // Reference to the empty card UI element.

        [SerializeField] TMP_Text itemName; // UI element for displaying the item's name.
        [SerializeField] Image itemIcon; // UI element for displaying the item's icon.

        [SerializeField] bool notDraggable; // Prevents items from being dragged into this slot if true.
        [SerializeField] bool isHotbarSlot;

        [Header("Selection")]
        [SerializeField] private GameObject selectionHighlight; // z.B. ein Rahmen, der standardmäßig deaktiviert ist


        [Header("Use Settings")]
        [SerializeField] private AudioClip useSound;        // Ton, der abgespielt werden soll
        [SerializeField] private bool   consumable = true; // ob das Item nach Gebrauch verschwindet

        private AudioSource audioSource;

        private bool isInUse = false;


        /// <summary>
        /// Initializes the card's state on awake.
        /// </summary>
        private void Awake()
        {
            if (useAsDrag)
            {
                // Set this card as the drag placeholder and deactivate it.
                ItemDraggingManager.dragCard = this;
                isOccupied = true;
                gameObject.SetActive(false);
            }

            // Refresh the display based on whether item data is present.
            if (itemData == null)
            {
                RefreshDisplay();
            }
            else
            {
                SetItem(itemData);
            }

            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Handles pointer down events to initiate dragging if allowed.
        /// </summary>
        /// <param name="eventData">Pointer event data.</param>
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            // Prevent dragging if the card is not occupied, not draggable, or used as a drag placeholder.
            if (useAsDrag || !isOccupied || notDraggable)
            {
                return;
            }

            // Set this card as the source of the drag operation.
            ItemDraggingManager.fromCard = this;
            TooltipManagerInventory.UnSetToolTip();
        }

        /// <summary>
        /// Handles pointer enter events to show tooltips and set the target card for dragging.
        /// </summary>
        /// <param name="eventData">Pointer event data.</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isOccupied)
            {
                // Show the tooltip for the item.
                TooltipManagerInventory.SetTooltip(itemData);

                // Set this card as the target for dragging if it is draggable.
                if (!notDraggable)
                {
                    ItemDraggingManager.toCard = ItemDraggingManager.fromCard;
                }

                return;
            }

            // Set this card as the target for dragging if it is draggable.
            if (!notDraggable)
            {
                ItemDraggingManager.toCard = this;
            }
        }

        /// <summary>
        /// Visuelles Ein-/Ausblenden des Auswahlrahmens.
        /// </summary>
        public void ToggleSelection(bool isSelected)
        {
            if (selectionHighlight != null)
            selectionHighlight.SetActive(isSelected);
        }

        /// <summary>
        /// Handles pointer exit events to hide the tooltip.
        /// </summary>
        /// <param name="eventData">Pointer event data.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isOccupied)
            {
                return;
            }

            // Hide the tooltip when the pointer exits the card.
            TooltipManagerInventory.UnSetToolTip();
        }

        /// <summary>
        /// Sets the item data for this card and updates the display.
        /// </summary>
        /// <param name="itemData">The item data to set.</param>
        /// <returns>True if the item was successfully set, false otherwise.</returns>
        public bool SetItem(InventoryItemData itemData)
        {
            // Prevent setting the item if the card is occupied and not a drag placeholder, or if the item data is null.
            if ((isOccupied && !useAsDrag) || itemData == null)
            {
                return false;
            }

            // Prevent placing items with an empty or null actionMethodName in the hotbar.
            if (isHotbarSlot && string.IsNullOrEmpty(itemData.actionMethodName))
            {
                Debug.LogWarning($"Item '{itemData.itemName}' cannot be placed in the hotbar because it has no assigned action.");
                return false;
            }

            // Update the card's item data and UI elements.
            this.itemData = itemData;
            this.itemName.text = itemData.name;
            this.itemIcon.sprite = itemData.itemIcon;

            this.isOccupied = true;

            RefreshDisplay();

            return true;
        }

        /// <summary>
        /// Clears the item data from this card and updates the display.
        /// </summary>
        public void UnSetItem()
        {
            itemData = null;
            this.isOccupied = false;

            RefreshDisplay();
        }

        /// <summary>
        /// Updates the card's display based on its occupied state.
        /// </summary>
        void RefreshDisplay()
        {
            // Show or hide the empty card UI element based on whether the card is occupied.
            emptyCard.SetActive(!isOccupied);
        }

        /// <summary>
        /// Executes the item's action if it has one
        /// </summary>
        public void UseItem()
        {

            if (isInUse) 
            return;

            if (itemData == null || string.IsNullOrEmpty(itemData.actionMethodName))
            {
                Debug.LogWarning("No item data or action assigned to this card.");
                return;
            }

            isInUse = true;

            // 1) Sound abspielen
            if (useSound != null)
            {
                if (audioSource != null)
                    audioSource.PlayOneShot(useSound);
                else
                    AudioSource.PlayClipAtPoint(useSound, Camera.main.transform.position);
            }

            // 2) Coroutine starten, die nach 3 Sekunden die Aktion ausführt
            StartCoroutine(DelayedUse());
        }

        private IEnumerator DelayedUse()
        {
            // 3 Sekunden warten
            yield return new WaitForSeconds(2f);

            // 3) Aktion ausführen
            HotBarActions.ExecuteAction(itemData.actionMethodName, itemName.text);

            // 4) Slot leeren, falls Verbrauchsobjekt
            if (consumable)
                UnSetItem();


            isInUse = false;
        }
    }
}