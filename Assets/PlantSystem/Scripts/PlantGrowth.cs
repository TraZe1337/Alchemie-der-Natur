using UnityEngine;

public class PlantGrowth : MonoBehaviour
{
    public PlantSO plantData;
    public Pot pot;
    public DehydrationColorChanger dehydrationColorChanger;

    private float currentGrowth = 0f;
    private float currentDehydration = 0f;
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

        CheckHydration();
        Consume();

        // Growth stage
        if (currentStage < plantData.MaxGrowthStage && currentGrowth >= (currentStage + 1) * 10f)
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

    private void CheckHydration()
    {
        // Calculate dehydration value based on waterFactor
        // The closer waterFactor is to 0, the higher the dehydrationValue
        float dehydrationRate = 1f - Mathf.Pow(pot.CurrentWaterLevel / plantData.minSoilMoisture, 2);
        // dehydrationValue between 0 and 1
        dehydrationRate = Mathf.Clamp01(dehydrationRate);
        //Debug.Log("Dehydration Value: " + dehydrationRate);

        if (dehydrationRate > 0) {
            currentDehydration += dehydrationRate * Time.deltaTime;
            if (currentDehydration >= plantData.dieTroughDehydrationThresholdinMinutes * 60f)
            {
                Die();
            }
            else
            {
                //TODO: When TimeManager gets slower it my take a while before dehydration effect is shown when new growth state is triggered.
                ShowDehydrationEffect(Mathf.Lerp(0f, 1f, currentDehydration / (plantData.dieTroughDehydrationThresholdinMinutes * 60f)));
            }
        }
        else
        {
            //TODO: Should the plants dehydration be reset to 0 directly when water is sufficient?
            currentDehydration = 0f; // Reset dehydration if water is sufficient
        }

        
    }

    private void SpawnNewPlantState(int state)
    {
        if (state < 0 || state >= plantData.MaxGrowthStage)
        {
            Debug.LogError("Invalid plant state: " + state);
            return;
        }

        // Destroy the previous plant instance if it exists
        if (currentPlantInstance != null)
        {
            Destroy(currentPlantInstance);
        }

        // Get the new plant prefab for the given state
        GameObject plantPrefab = plantData.GetPlantPrefab(state);
        if (plantPrefab != null)
        {
            // Instantiate the new plant prefab and store the reference
            currentPlantInstance = Instantiate(plantPrefab, transform);
            currentPlantInstance.transform.SetParent(transform);
        }
        else
        {
            Debug.LogError("No prefab found for state: " + state);
        }
    }

    private void ShowDehydrationEffect(float effectIntensity)
    {
        // Implement visual or audio effects for dehydration here
        //Debug.Log("Dehydration effect triggered for: " + gameObject.name);
        dehydrationColorChanger.UpdateDehydrationColor(effectIntensity);
    }

    private void Die()
    {
        Debug.Log("Plant died: " + gameObject.name);

        // Destroy any current plant instance
        if (currentPlantInstance != null)
            Destroy(currentPlantInstance);

        // Destroy this PlantGrowth game object
        Destroy(gameObject);
    }

    private void Consume() {
        //TODO: Add nutrient consumption logic here
        // Consume water and nutrients based on the growth rate
        float waterConsumption = plantData.waterConusmptionRate * Time.deltaTime;
        //float nutrientConsumption = plantData.nutrientRequirement * Time.deltaTime;

        pot.ConsumeWater(waterConsumption);
        //pot.ConsumeNutrients(nutrientConsumption);
        Debug.Log("Water Level: " + pot.CurrentWaterLevel + " Nutrient Level: " + pot.CurrentNutrientLevel);
    }
}
