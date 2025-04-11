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

    // For water and sunlight, this method now incorporates the upper requirement.
    private float CalculateEnvironmentalFactor(float current, float minRequirement, float minPreference, float maxPreference, float maxRequirement)
    {
        if (current < minRequirement)
            return 0f; // Too little: no growth.

        if (current < minPreference)
            return (current - minRequirement) / (minPreference - minRequirement);

        if (current <= maxPreference)
            return 1f; // Optimal growth conditions.

        if (current < maxRequirement)
            return (maxRequirement - current) / (maxRequirement - maxPreference);

        return 0f; // Exceeds upper bound: no growth.
    }

    // For nutrients, since there's no upper limit, we use the simpler calculation.
    float CalculateNutriScore(float current, float minRequirement, float minPreference)
    {
        if (current < minRequirement)
            return 0f;

        if (current < minPreference)
            return (current - minRequirement) / (minPreference - minRequirement);

        return 1f;
    }

    // Called each frame
    public void TickGrowth(float deltaTime)
    {
        float waterLevel = pot.CurrentWaterLevel;
        float sunlightLevel = pot.CurrentSunlightLevel;
        float nutrientLevel = pot.CurrentNutrientLevel;

        // Calculate satisfaction factors using the updated formulas.
        float waterFactor = CalculateEnvironmentalFactor(waterLevel,
                             plantData.minMoistureRequirement,
                             plantData.minMoisturePreference,
                             plantData.maxMoisturePreference,
                             plantData.maxMoistureRequirement);

        float sunlightFactor = CalculateEnvironmentalFactor(sunlightLevel,
                               plantData.minSunlightRequirement,
                               plantData.minSunlightPreference,
                               plantData.maxSunlightPreference,
                               plantData.maxSunlightRequirement);

        float nutriScore = CalculateNutriScore(nutrientLevel,
                               plantData.minSunlightRequirement,
                               plantData.minSunlightPreference);

        // Combine the factors with a weighted geometric mean.
        // Weights: Water = 0.5, Sunlight = 0.3, Nutrients = 0.2.
        float overallFactor = Mathf.Pow(waterFactor, 0.5f) *
                              Mathf.Pow(sunlightFactor, 0.3f) *
                              Mathf.Pow(nutriScore, 0.2f);

        // Compute current growth speed in units per second.
        float growthSpeed = plantData.growthRate * overallFactor;

        // Apply growth over the deltaTime.
        currentGrowth += growthSpeed * deltaTime;
        Debug.Log("Current GrowthSpeed: " + growthSpeed + " Current Growth: " + currentGrowth + " Current Stage: " + currentStage);

        CheckHydration();
        //Consume();

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
        //// Calculate dehydration value based on waterFactor
        //// The closer waterFactor is to 0, the higher the dehydrationValue
        //float dehydrationRate = 1f - Mathf.Pow(pot.CurrentWaterLevel / plantData.minSoilMoisture, 2);
        //// dehydrationValue between 0 and 1
        //dehydrationRate = Mathf.Clamp01(dehydrationRate);
        ////Debug.Log("Dehydration Value: " + dehydrationRate);
//
        //if (dehydrationRate > 0)
        //{
        //    currentDehydration += dehydrationRate * Time.deltaTime;
        //    if (currentDehydration >= plantData.dieTroughDehydrationThresholdinMinutes * 60f)
        //    {
        //        Die();
        //    }
        //    else
        //    {
        //        //TODO: When TimeManager gets slower it my take a while before dehydration effect is shown when new growth state is triggered.
        //        ShowDehydrationEffect(Mathf.Lerp(0f, 1f, currentDehydration / (plantData.dieTroughDehydrationThresholdinMinutes * 60f)));
        //    }
        //}
        //else
        //{
        //    //TODO: Should the plants dehydration be reset to 0 directly when water is sufficient?
        //    currentDehydration = 0f; // Reset dehydration if water is sufficient
        //}


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

    private void Consume()
    {
        //TODO: Add nutrient consumption logic here
        // Consume water and nutrients based on the growth rate
        float waterConsumption = plantData.waterConusmptionRate * Time.deltaTime;
        //float nutrientConsumption = plantData.nutrientRequirement * Time.deltaTime;

        pot.ConsumeWater(waterConsumption);
        //pot.ConsumeNutrients(nutrientConsumption);
        //Debug.Log("Water Level: " + pot.CurrentWaterLevel + " Nutrient Level: " + pot.CurrentNutrientLevel);
    }
}
