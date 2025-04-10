using UnityEngine;

public class PlantGrowth : MonoBehaviour
{
    public PlantSO plantData;
    public Pot pot;

    private float currentGrowth = 0f;
    private int currentStage = 0;
    private GameObject currentPlantInstance;

    private void OnEnable()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.RegisterPlant(this);
    }

    private void OnDisable()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.UnregisterPlant(this);
    }

    // Called each frame
    public void TickGrowth(float deltaTime)
    {
        float waterFactor = pot.CurrentWaterLevel / plantData.waterRequirement;
        float growthSpeed = plantData.growthRate * Mathf.Clamp01(waterFactor);
        currentGrowth += growthSpeed * deltaTime;

        // Growth stage
        if (currentStage < plantData.MaxStage && currentGrowth >= (currentStage + 1) * 10f)
        {
            AdvanceGrowthStage();
        }
    }

    private void AdvanceGrowthStage()
    {
        Debug.Log("Advancing growth stage: " + currentStage);
        SpawnNewPlantState(currentStage);
        currentStage++;
    }

    private void SpawnNewPlantState(int state) {
        if (state < 0 || state >= plantData.MaxStage) {
            Debug.LogError("Invalid plant state: " + state);
            return;
        }

        // Destroy the previous plant instance if it exists
        if (currentPlantInstance != null) {
            Destroy(currentPlantInstance);
        }

        // Get the new plant prefab for the given state
        GameObject plantPrefab = plantData.GetPlantPrefab(state);
        if (plantPrefab != null) {
            // Instantiate the new plant prefab and store the reference
            currentPlantInstance = Instantiate(plantPrefab, transform);
            currentPlantInstance.transform.SetParent(transform);
        } else {
            Debug.LogError("No prefab found for state: " + state);
        }
    }
}
