using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using StarterAssets;

public class HealthStatusUI : MonoBehaviour
{
    public Transform mainCamera;
    [SerializeField] private Pot pot;
    [SerializeField] private PlantGrowth growth;
    public List<UnityEngine.UI.Image> images;
    private List<EffectSO> negativeHealthEffects;
    [SerializeField] private GameObject plantPreviewGameObject;
    [SerializeField] private GameObject plantPreviewCamera;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject player;

    private UIDocument plantPreviewUIDocument;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plantPreviewCamera.SetActive(false);
        plantPreviewUIDocument = plantPreviewGameObject.GetComponent<UIDocument>();
        negativeHealthEffects = new List<EffectSO>();
        // Get Main Camera transform
        mainCamera = Camera.main?.transform; // Safely get the main camera's transform
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found. Please ensure a camera is tagged as 'MainCamera'.");
        }
        ClearImages();
        gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");

        crosshair = GameObject.FindGameObjectWithTag("Crosshair");
    }

    public void AddEffect(List<EffectSO> effects)
    {
        if (!EffectsMatch(effects))
        {
            negativeHealthEffects = new List<EffectSO>(effects);
            RestockEffectImages();
        }
    }

    private List<VisualElement> FillUIDocumentEffectImagesList() {
        List<VisualElement> effectImagesInUIDocument = new List<VisualElement>();  
        VisualElement root = plantPreviewUIDocument.rootVisualElement;
        effectImagesInUIDocument.Add(root.Q<VisualElement>("Middle"));
        effectImagesInUIDocument.Add(root.Q<VisualElement>("Left"));
        effectImagesInUIDocument.Add(root.Q<VisualElement>("Right"));

        return effectImagesInUIDocument;
    }
    
    public void ClearEffects()
    {
        ClearImages();
        ClearUIEffects();
        negativeHealthEffects.Clear();   
    }

    private void ClearImages()
    {
        foreach (var image in images)
        {
            image.enabled = false;
        }
    }

    private List<VisualElement> ClearUIEffects()
    {
        List<VisualElement> uIIamgesList = new List<VisualElement>();
        if (plantPreviewGameObject.activeSelf)
        {
            uIIamgesList = FillUIDocumentEffectImagesList();
            foreach (VisualElement image in uIIamgesList)
            {
                image.style.backgroundImage = new StyleBackground((Texture2D)null);
                image.style.backgroundColor = new StyleColor(Color.white);
            }
            return uIIamgesList;
        }
        return null;
    }
    private void RestockEffectImages() {
        List<VisualElement> uIIamgesList;
        // Clear existing images
        ClearImages();
        uIIamgesList = ClearUIEffects();
        for (int i = 0; i < negativeHealthEffects.Count; i++)
        {
            if (negativeHealthEffects[i] == null)
                continue; // Skip null effects
            
            // Check if the effect image is not null or empty
            if (negativeHealthEffects[i].effectImage != null)
            {
                images[i].sprite = negativeHealthEffects[i].effectImage;
                images[i].enabled = true;

                if (uIIamgesList != null) {
                    uIIamgesList[i].style.backgroundImage = negativeHealthEffects[i].effectImage.texture;
                }
            }
        }
    }

    private bool EffectsMatch(List<EffectSO> effects)
    {
        if (effects == null || negativeHealthEffects == null)
            return false;
        
        if (effects.Count != negativeHealthEffects.Count)
            return false;
        
        // Create a HashSet for effect types from the parameter list.
        HashSet<PlantHealthStages> effectTypes = new HashSet<PlantHealthStages>();
        foreach (var effect in effects)
        {
            effectTypes.Add(effect.effectType);
        }
        
        // Create a HashSet for effect types from the negativeHealthEffects list.
        HashSet<PlantHealthStages> uiEffectTypes = new HashSet<PlantHealthStages>();
        foreach (var effect in negativeHealthEffects)
        {
            uiEffectTypes.Add(effect.effectType);
        }
        
        return effectTypes.SetEquals(uiEffectTypes);
    }

    private void SetupPlantPreviewUI() {
        VisualElement root = plantPreviewUIDocument.rootVisualElement;
        root.Q<Label>("PlantName").text = pot.PlantName;
        root.Q<Label>("PlantInfoText").text = pot.PlantDescription;

        VisualElement quitBtn = root.Q<VisualElement>("CloseButton");
        quitBtn.RegisterCallback<ClickEvent>(ev => {ClosePlantPreviewUI();});

        UpdatePlantPreviewUI();
    }

    private void UpdatePlantPreviewUI() {
        VisualElement root = plantPreviewUIDocument.rootVisualElement;
        ProgressBar healthBar = root.Q<ProgressBar>("PlantHealthProgress");
        healthBar.lowValue  = 0;
        healthBar.highValue = 100;
        healthBar.value     = growth.currentHealth;
        healthBar.title = healthBar.value.ToString("F0");

        ProgressBar nutritBar = root.Q<ProgressBar>("NutriScoreProgress");
        nutritBar.lowValue  = 0;
        nutritBar.highValue = pot.MaxNutrientsLevel;
        nutritBar.value     = pot.CurrentNutrientLevel;
        nutritBar.title = nutritBar.value.ToString("F0");

        ProgressBar waterBar = root.Q<ProgressBar>("WaterLvlProgress");
        waterBar.lowValue  = 0;
        waterBar.highValue = pot.MaxWaterLevel;
        waterBar.value     = pot.CurrentWaterLevel;
        waterBar.title = waterBar.value.ToString("F0");

        Label sunlightLabel = root.Q<Label>("SunlightLvlLabel");
        sunlightLabel.text = pot.CurrentSunlightLevel.ToString("F0");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (plantPreviewGameObject.activeSelf)
            {
                ClosePlantPreviewUI();
            }
            else
            {
                player.GetComponent<StarterAssetsInputs>().cursorInputForLook = false;
                player.GetComponent<StarterAssetsInputs>().cursorLocked = false;
                crosshair.SetActive(false);
                growth = pot.GetComponentInChildren<PlantGrowth>();
                plantPreviewCamera.SetActive(true);
                plantPreviewGameObject.SetActive(true);
                RestockEffectImages();
                SetupPlantPreviewUI();
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
            }
        }

        if (plantPreviewGameObject.activeSelf)
        {
            UpdatePlantPreviewUI();
        }
    }

    private void ClosePlantPreviewUI()
    {
        plantPreviewCamera.SetActive(false);
        plantPreviewGameObject.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        player.GetComponent<StarterAssetsInputs>().cursorInputForLook = true;
        player.GetComponent<StarterAssetsInputs>().cursorLocked = true;
        crosshair.SetActive(true);
    }
    void LateUpdate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - mainCamera.position);
        transform.rotation = targetRotation;
    }

}
