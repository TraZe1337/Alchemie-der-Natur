using UnityEngine;

public interface IUsable
{
    void AddWater(float amount);
    void AddNutrients(float amount);
    (int, PlantSO) HarvestPotPlant();
}
