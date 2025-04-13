using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class Recipe
{
    public string title;
    public string description;
    public string[] ingredients;
}

[System.Serializable]
public class RecipeBook
{
    public Recipe[] recipes;
}

public class PageSwitcher_dynamic : MonoBehaviour
{
    [SerializeField] private GameObject pagePrefab; // Prefab for a single page
    [SerializeField] private Transform pageContainer; // Parent object for pages
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    [SerializeField] private TextMeshProUGUI pageNumberText;
    [SerializeField] private TextAsset recipeJson; // JSON file containing recipes

    private List<GameObject> pages = new List<GameObject>();
    private int currentPageIndex = 0;

    void Start()
    {
        LoadRecipes();
        UpdatePageVisibility();
        nextButton.onClick.AddListener(SwitchToNextPage);
        prevButton.onClick.AddListener(SwitchToPreviousPage);
    }

    private void LoadRecipes()
    {
        if (recipeJson == null)
        {
            Debug.LogError("Recipe JSON file is not assigned!");
            return;
        }

        // Parse the JSON file
        RecipeBook recipeBook = JsonUtility.FromJson<RecipeBook>(recipeJson.text);

        // Create pages dynamically
        for (int i = 0; i < recipeBook.recipes.Length; i += 2)
        {
            GameObject page = Instantiate(pagePrefab, pageContainer);
            if (page == null)
            {
                Debug.LogError("Page prefab is not assigned or could not be instantiated!");
                continue;
            }

            page.SetActive(false);

            // Populate the page with up to two recipes
            Transform entry1 = page.transform.Find("Vertical Group/Entry");
            if (entry1 != null)
            {
                TextMeshProUGUI titleText1 = entry1.Find("Title1")?.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI descriptionText1 = entry1.Find("Grid/Description1")?.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI ingredientsText1 = entry1.Find("Grid/Ingredients1")?.GetComponent<TextMeshProUGUI>();

                if (titleText1 != null && descriptionText1 != null && ingredientsText1 != null)
                {
                    Recipe recipe1 = recipeBook.recipes[i];
                    titleText1.text = recipe1.title;
                    descriptionText1.text = recipe1.description;
                    ingredientsText1.text = string.Join("\n", recipe1.ingredients);
                    Debug.Log($"Populated Recipe 1: {recipe1.title}");
                }
                else
                {
                    Debug.LogError("Failed to find or populate Recipe 1 UI elements!");
                }
            }

            if (i + 1 < recipeBook.recipes.Length)
            {
                Transform entry2 = page.transform.Find("Vertical Group/Entry (1)");
                if (entry2 != null)
                {
                    TextMeshProUGUI titleText2 = entry2.Find("Title2")?.GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI descriptionText2 = entry2.Find("Grid/Description2")?.GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI ingredientsText2 = entry2.Find("Grid/Ingredients2")?.GetComponent<TextMeshProUGUI>();

                    if (titleText2 != null && descriptionText2 != null && ingredientsText2 != null)
                    {
                        Recipe recipe2 = recipeBook.recipes[i + 1];
                        titleText2.text = recipe2.title;
                        descriptionText2.text = recipe2.description;
                        ingredientsText2.text = string.Join("\n", recipe2.ingredients);
                        Debug.Log($"Populated Recipe 2: {recipe2.title}");
                    }
                    else
                    {
                        Debug.LogError("Failed to find or populate Recipe 2 UI elements!");
                    }
                }
            }

            pages.Add(page);
        }
    }

    private void UpdatePageVisibility()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == currentPageIndex);
        }

        prevButton.interactable = currentPageIndex > 0;
        nextButton.interactable = currentPageIndex < pages.Count - 1;

        if (pageNumberText != null)
        {
            pageNumberText.text = $"Page {currentPageIndex + 1} of {pages.Count}";
        }
    }

    private void SwitchToNextPage()
    {
        if (currentPageIndex < pages.Count - 1)
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
