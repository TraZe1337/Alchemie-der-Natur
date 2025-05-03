using UnityEngine;
using System.Collections.Generic;
using System;

public class PlantGrowth : MonoBehaviour
{
    public PlantSO plantData;
    public Pot pot;
    public NegativeHealthEffectVisualizer negativeHealthEffectVisualizer;
    public HealthStatusUI healthStatusUI;

    public List<EffectSO> negativeHealthEffectList;

    private float currentGrowth = 0f;
    private float plantDeathRate = 0f;

    private int currentStage = 0;
    private GameObject currentPlantInstance;

    private List<EffectSO> currentNegativeHealthEffects;

    [Header("Stress & Health Settings")]
    [SerializeField] private float weightWater = 0.5f;
    [SerializeField] private float weightSunlight = 0.3f;
    [SerializeField] private float weightNutrients = 0.2f;

    private float cumulativeStressArea = 0f;
    private float cumulativeHealArea = 0f;
    public float currentHealth = 100f;    // 0 to 100

    void Start()
    {
        currentNegativeHealthEffects = new List<EffectSO>();
    }

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
    private float CalculateWaterFactor(float current, float minRequirement, float minPreference, float maxPreference, float maxRequirement)
    {
        if (current < minRequirement)
        {
            currentNegativeHealthEffects.Add(GetEffectSO(PlantHealthStages.Dehydration));
            return 0f; // Too little: no growth.
        }

        if (current < minPreference)
        {
            currentNegativeHealthEffects.Add(GetEffectSO(PlantHealthStages.MoreWaterPrefered));
            return (current - minRequirement) / (minPreference - minRequirement);
        }

        if (current <= maxPreference)
            return 1f; // Optimal growth conditions.

        if (current < maxRequirement)
        {
            currentNegativeHealthEffects.Add(GetEffectSO(PlantHealthStages.LessWaterPrefered));
            return (maxRequirement - current) / (maxRequirement - maxPreference);
        }

        currentNegativeHealthEffects.Add(GetEffectSO(PlantHealthStages.Overwatered));
        return 0f; // Exceeds upper bound: no growth.
    }

    private float CalculateSunlightFactor(float current, float minRequirement, float minPreference, float maxPreference, float maxRequirement)
    {
        if (current < minRequirement)
        {
            currentNegativeHealthEffects.Add(GetEffectSO(PlantHealthStages.InTheDark));
            return 0f; // Too little: no growth.
        }

        if (current < minPreference)
        {
            currentNegativeHealthEffects.Add(GetEffectSO(PlantHealthStages.MoreSunlightPrefered));
            return (current - minRequirement) / (minPreference - minRequirement);
        }

        if (current <= maxPreference)
            return 1f; // Optimal growth conditions.


        if (current < maxRequirement)
        {
            currentNegativeHealthEffects.Add(GetEffectSO(PlantHealthStages.LessSunlightPrefered));
            return (maxRequirement - current) / (maxRequirement - maxPreference);
        }

        currentNegativeHealthEffects.Add(GetEffectSO(PlantHealthStages.OverexposedToSunlight));
        return 0f; // Exceeds upper bound: no growth.
    }

    // For nutrients, since there's no upper limit, we use the simpler calculation.
    float CalculateNutriScore(float current, float minRequirement, float minPreference)
    {
        if (current < minRequirement) {
            currentNegativeHealthEffects.Add(GetEffectSO(PlantHealthStages.NutrientDeficiency));
            return 0f;
        }
             

        if (current < minPreference) {
            currentNegativeHealthEffects.Add(GetEffectSO(PlantHealthStages.MoreNutrientsPrefered));
            return (current - minRequirement) / (minPreference - minRequirement);
        }

        return 1f;
    }

    // Called each frame
    public void TickGrowth(float deltaTime)
    {
                        //Debug.Log("negativeHealthEffectList length:" + negativeHealthEffectList.Count);
        currentNegativeHealthEffects.Clear();
        float waterLevel = pot.CurrentWaterLevel;
        float sunlightLevel = pot.CurrentSunlightLevel;
        float nutrientLevel = pot.CurrentNutrientLevel;

        // Calculate satisfaction factors.
        float waterFactor = CalculateWaterFactor(waterLevel,
                             plantData.minMoistureRequirement,
                             plantData.minMoisturePreference,
                             plantData.maxMoisturePreference,
                             plantData.maxMoistureRequirement);

        float sunlightFactor = CalculateSunlightFactor(sunlightLevel,
                               plantData.minSunlightRequirement,
                               plantData.minSunlightPreference,
                               plantData.maxSunlightPreference,
                               plantData.maxSunlightRequirement);

        float nutriScore = CalculateNutriScore(nutrientLevel,
                               plantData.minSunlightRequirement,
                               plantData.minSunlightPreference);

        float stressLevel = weightWater * (1f - waterFactor)
                          + weightSunlight * (1f - sunlightFactor)
                          + weightNutrients * (1f - nutriScore);
        
        float stressOrHealLevel = (stressLevel == 0f) ? -plantData.healingRate  :  stressLevel;
        
        // Positive = stress, Negative = healing
        cumulativeStressArea += Mathf.Max(0f, stressOrHealLevel) * deltaTime;
        cumulativeHealArea   += Mathf.Max(0f, -stressOrHealLevel)  * deltaTime;

        currentHealth = Mathf.Clamp(100f - cumulativeStressArea + cumulativeHealArea, 0f, 100f);

        if (currentStage < plantData.MaxGrowthStage)
        {
            // Combine the factors with a weighted geometric mean.
            // Weights: Water = 0.5, Sunlight = 0.3, Nutrients = 0.2.
            float overallFactor = Mathf.Pow(waterFactor, weightWater) *
                                  Mathf.Pow(sunlightFactor, weightSunlight) *
                                  Mathf.Pow(nutriScore, weightNutrients);

            // Compute current growth speed in units per second.
            float growthSpeed = plantData.growthRate * overallFactor;

            // Apply growth over the deltaTime.
            currentGrowth += growthSpeed * deltaTime;
            //Debug.Log("Current GrowthSpeed: " + growthSpeed + " Current Growth: " + currentGrowth + " Current Stage: " + currentStage);

            //Consume();
            
            // Growth stage
            if (currentGrowth >= (currentStage + 1) * 10f)
            {
                AdvanceGrowthStage();
            }
        }
        else
        {
            //Plant is fully grown
            //TODO: Check if fully grown plant has enough water, nutrients and sunlight. If not, show dehydration effect.
            Debug.Log("Plant is fully grown: " + currentStage);
        }
        HealthCheck();
    }

    private void AdvanceGrowthStage()
    {
        Debug.Log("Advancing growth stage: " + currentStage);
        SpawnNewPlantState(currentStage);
        currentStage++;
    }

    private void HealthCheck()
    {
        if (currentHealth <= 0f)
        {
            Die();
            return;
        }

        if (currentNegativeHealthEffects.Count > 0)
        {
            foreach (EffectSO effect in currentNegativeHealthEffects)
            {
                // TOTO: When death effect the effect is showed via color change, via icon is shown by ever effect
                PlantHealthStages currentStage = effect.effectType;
                switch (currentStage)
                {
                    case PlantHealthStages.Dehydration:
                        CalcNegativeHealthEffect(currentStage, pot.CurrentWaterLevel, plantData.minMoistureRequirement, plantData.maxMoistureRequirement);
                        break;
                    case PlantHealthStages.InTheDark:
                        CalcNegativeHealthEffect(currentStage, pot.CurrentSunlightLevel, plantData.minSunlightRequirement, plantData.maxSunlightRequirement);
                        break;
                    case PlantHealthStages.NutrientDeficiency:
                        CalcNegativeHealthEffect(currentStage, pot.CurrentNutrientLevel, plantData.minNutrientsRequirement, 0f);
                        break;
                    case PlantHealthStages.Overwatered:
                        CalcNegativeHealthEffect(currentStage, pot.CurrentWaterLevel, plantData.minMoistureRequirement, plantData.maxMoistureRequirement);
                        break;
                    case PlantHealthStages.OverexposedToSunlight:
                        CalcNegativeHealthEffect(currentStage, pot.CurrentSunlightLevel, plantData.minSunlightRequirement, plantData.maxSunlightRequirement);
                        break;
                    default:
                        Debug.LogWarning("Unhandled health stage: " + currentStage);
                        break;
                }
                healthStatusUI.AddEffect(currentNegativeHealthEffects);
            }
        } else {
            // Clear all effects if no negative health effects are present.
            healthStatusUI.ClearEffects();
        }
    }

    // Spawns a new plant prefab based on the current growth stage
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
            currentPlantInstance.layer = gameObject.layer; // Set the layer to match the parent for PlantPreview Cam
        }
        else
        {
            Debug.LogError("No prefab found for state: " + state);
        }
    }


    private void CalcNegativeHealthEffect(PlantHealthStages healthStage, float currentVal, float lowerLimit, float upperLimit)
    {
        float negativeHealthRate = 0f;
        if (currentVal < lowerLimit)
        {
            negativeHealthRate = Mathf.Clamp01(1f - Mathf.Pow(currentVal / lowerLimit, 2));
        }
        else if (currentVal > upperLimit)
        {
            negativeHealthRate = Mathf.Clamp01(Mathf.Pow(currentVal / upperLimit, 2));
        }

        plantDeathRate += negativeHealthRate * Time.deltaTime;
        //Debug.Log("plantDeathRate: " + plantDeathRate + "with rate of: " + negativeHealthRate);
        negativeHealthEffectVisualizer.UpdateHealthEffectColor(
                                        Mathf.Lerp(0f, 1f, plantDeathRate / (plantData.robustness * 60f)));

        RemoveTheDead();
    }

    private void RemoveTheDead()
    {

        if (plantDeathRate >= plantData.robustness * 60f)
        {
            Die();
        }
    }


    private void Die()
    {
        Debug.Log("Plant died: " + gameObject.name);
        Destroy(healthStatusUI.gameObject);
        // Destroy any current plant instance
        if (currentPlantInstance != null)
            Destroy(currentPlantInstance);

        // Destroy this PlantGrowth game object
        Destroy(gameObject);
    }

    private void Consume()
    {
        //TODO: Consume water and nutrients based on the growth rate
        float waterConsumption = plantData.waterConusmptionRate * Time.deltaTime;
        float nutrientConsumption = plantData.nutrientConsumptionRate * Time.deltaTime;

        pot.ConsumeWater(waterConsumption);
        pot.ConsumeNutrients(nutrientConsumption);
        //Debug.Log("Water Level: " + pot.CurrentWaterLevel + " Nutrient Level: " + pot.CurrentNutrientLevel);
    }

    public int Harvest()
    {
        Die();
        // TODO: Insert harvest into inventory
        return Mathf.RoundToInt(plantData.harvestYield * (currentHealth / 100f));
    }

    private EffectSO GetEffectSO(PlantHealthStages healthStage)
    {
        foreach (EffectSO effect in negativeHealthEffectList)
        {
            if (effect.effectType == healthStage)
            {
                return effect;
            }
        }
        return null;
    }
}
