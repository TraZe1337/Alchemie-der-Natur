using UnityEngine;
using UnityEngine.UI;
using RedstoneinventeGameStudio;

public class HotBarManager : MonoBehaviour
{

    [Header("Hotbar Slots")]
    [SerializeField] private CardManager[] hotbarCardManagers = new CardManager[5]; // Corresponding CardManagers

    [Header("Debug")]
    [SerializeField] private bool showDebugMessages = true;


    [SerializeField] private RectTransform selectionHighlight;

    private int selectedIndex = 0;


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
        SelectSlot(0);
    }

     /// <summary>
    /// Forwardet den Aufruf an den CardManager des aktuell ausgewählten Slots.
    /// </summary>
    public void UseHotbarItem(int slotIndex)
    {
        var card = hotbarCardManagers[slotIndex];
        if (card != null && card.isOccupied && card.itemData != null)
        {
            if (showDebugMessages)
                Debug.Log($"Benutze Hotbar Slot {slotIndex}");
            card.UseItem();
        }
        else if (showDebugMessages)
        {
            Debug.Log($"Slot {slotIndex} ist leer oder nicht bevölkert");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 1.–5. Slot auswählen mit den Zahlentasten
        for (int i = 0; i < hotbarCardManagers.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSlot(i);
            }
        }

        // Benutzen-Taste (hier E), nutzt das gerade ausgewählte Item
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseHotbarItem(selectedIndex);
        }
    }
    
     /// <summary>
    /// Hebt die Auswahl am alten Slot auf und setzt sie am neuen.
    /// </summary>
    private void SelectSlot(int newIndex)
    {
        if (newIndex < 0 || newIndex >= hotbarCardManagers.Length) return;
        var slotRT = hotbarCardManagers[newIndex].GetComponent<RectTransform>();
        if (slotRT == null) return;

        // 1) Parent wechseln (false = behält lokale Werte bei)
        selectionHighlight.SetParent(slotRT, false);

        // 2) Hinter alle anderen Kinder schieben
        selectionHighlight.SetAsFirstSibling();

        // 3) Voll auf den Slot stretchen
        selectionHighlight.anchorMin = Vector2.zero;
        selectionHighlight.anchorMax = Vector2.one;
        selectionHighlight.anchoredPosition = Vector2.zero;
        selectionHighlight.sizeDelta = Vector2.zero;

        // Optional: Debug
        if (showDebugMessages)
            Debug.Log($"Highlight unter Slot {newIndex} verschoben");
    }
}
