using System.Collections.Generic;
using RedstoneinventeGameStudio;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantSO", menuName = "Scriptable Objects/PlantSO")]
public class PlantSO : ScriptableObject
{
    [Header("General Settings")]
    public string plantName;
    public string plantDescription;
    public InventoryItemData plantItemData; // Item data for the plant, used for inventory and UI representation.
    public InventoryItemData seedItemData; // Item data for the seed, used for inventory and UI representation.
    public List<GameObject> plantPrefabs;

    [Header("Water Settings")]
    // Minimal Optimal water required for full growth speed.
    public float minMoisturePreference;
    public float maxMoisturePreference;
    [Min(0.1f)]
    // Minimum moisture level in the soil that the plant can tolerate before it starts to dehydrate.
    public float minMoistureRequirement;
    public float maxMoistureRequirement;
    // Amount of water consumed per TimeManager Tick.
    [Min(0.1f)]
    public float waterConusmptionRate;

    [Header("Environmental Requirements")]
    public float minSunlightPreference;
    public float maxSunlightPreference;
    public float minSunlightRequirement;
    public float maxSunlightRequirement;

    [Header("Nutrient Settings")]
    // Nutrient amount in soil for extra growth speed.
    public float minNutrientsPreference;
    public float minNutrientsRequirement;
    public float nutrientConsumptionRate;

    [Header("Production & Growth")]
    public float harvestYield;
     // Growth rate per second when conditions are optimal.
    public float growthRate;

    [Header("Plant Health")]
    //Time in minutes a plant dies when it's requirements is not met.
    public float robustness;
    //Value added per seconds a plant gets to regain full harvest yield after beeing mistreated.
    public float healingRate;

    // Max Stage of the plant
    // This is the number of different growth stages the plant has.
    public int MaxGrowthStage{
        get
        {
            return plantPrefabs.Count;
        }
    }

    // This method returns the prefab for the given growth stage.
    public GameObject GetPlantPrefab(int stage)
    {
        if (stage < 0 || stage >= MaxGrowthStage)
        {
            Debug.LogError("Plant fully grown: " + stage);
            return null;
        }
        return plantPrefabs[stage];
    }

    // Validation to ensure minSoilMoisture is not greater than waterRequirement.
    private void OnValidate()
    {
        //if (minSoilMoisture > minMoisturePreference)
        //{
        //    Debug.LogWarning("minSoilMoisture cannot be greater than waterRequirement in " + name + ". Adjusting minSoilMoisture to waterRequirement.");
        //    minSoilMoisture = minMoisturePreference;
        //}
//
        //if (minMoisturePreference <= 0)
        //{
        //    Debug.LogWarning("waterRequirement cannot be negative or 0 in " + name + ". Setting it to 0.5.");
        //    minMoisturePreference = 0.5f;
        //}'
    }
}
