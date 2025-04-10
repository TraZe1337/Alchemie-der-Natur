using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantSO", menuName = "Scriptable Objects/PlantSO")]
public class PlantSO : ScriptableObject
{
    public string plantName;
    public List<GameObject> plantPrefabs;
    public float waterRequirement;
    public float sunlightRequirement;
    public float nutrientRequirement;
    public float harvestYield;
    public float growthRate;

    // Max Stage of the plant
    // This is the number of different growth stages the plant has.
    public int MaxStage{
        get
        {
            return plantPrefabs.Count;
        }
    }

    // This method returns the prefab for the given growth stage.
    public GameObject GetPlantPrefab(int stage)
    {
        if (stage < 0 || stage >= MaxStage)
        {
            Debug.LogError("Plant fully grown: " + stage);
            return null;
        }
        return plantPrefabs[stage];
    }
}
