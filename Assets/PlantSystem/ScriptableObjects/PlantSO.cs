using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantSO", menuName = "Scriptable Objects/PlantSO")]
public class PlantSO : ScriptableObject
{
    public string plantName;
    public List<GameObject> plantPrefabs;
    // Optimal water required for full growth speed.
    public float waterRequirement;
    // Max Value (in Minutes) the plant can survive without any water.
    public float dieTroughDehydrationThresholdinMinutes;
    public float sunlightRequirement;
    public float nutrientRequirement;
    public float harvestYield;
     // Growth rate per second when conditions are optimal.
    public float growthRate;

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
}
