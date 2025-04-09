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
}
