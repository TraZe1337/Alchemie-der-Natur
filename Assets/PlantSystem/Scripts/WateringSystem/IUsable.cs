using UnityEngine;

public interface IUsable
{
    void AddWater(float amount);
    void AddNutrients(float amount);
    (int, int, PlantSO) HarvestPotPlant();
}
