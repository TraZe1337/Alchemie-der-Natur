using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace

public class PageSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] pages; // Array of page canvases
    [SerializeField] private Button nextButton; // Button to go to the next page
    [SerializeField] private Button prevButton; // Button to go to the previous page
    [SerializeField] private TextMeshProUGUI pageNumberText; // TextMeshPro object to display the current page number

    private int currentPageIndex = 0;

    void Start()
    {
        // Initialize buttons and pages
        UpdatePageVisibility();
        nextButton.onClick.AddListener(SwitchToNextPage);
        prevButton.onClick.AddListener(SwitchToPreviousPage);
    }

    private void UpdatePageVisibility()
    {
        // Activate the current page and deactivate others
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPageIndex);
        }

        // Enable or disable buttons based on page availability
        prevButton.interactable = currentPageIndex > 0;
        nextButton.interactable = currentPageIndex < pages.Length - 1;

        // Update the page number text
        if (pageNumberText != null)
        {
            pageNumberText.text = $"Page {currentPageIndex + 1} of {pages.Length}";
        }
    }

    private void SwitchToNextPage()
    {
        if (currentPageIndex < pages.Length - 1)
        {
            currentPageIndex++;
            UpdatePageVisibility();
        }
    }

    private void SwitchToPreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdatePageVisibility();
        }
    }
}
