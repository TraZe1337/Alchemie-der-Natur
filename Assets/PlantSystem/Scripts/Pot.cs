using UnityEngine;

public class Pot : MonoBehaviour
{
    [SerializeField] private float waterLevel = 100f;
    [SerializeField] private float sunlightLevel = 1f;
    [SerializeField] private float nutrientLevel = 50f;

    public float CurrentWaterLevel => waterLevel;
    public float CurrentSunlightLevel => sunlightLevel;
    public float CurrentNutrientLevel => nutrientLevel;


    void Update()
    {
        //UpdateEnvironment(Time.deltaTime);
    }

    public void UpdateEnvironment(float deltaTime)
    {
        // Example: water evaporation over time
        waterLevel = Mathf.Max(0, waterLevel - 0.5f * deltaTime);
    }

    public void AddWater(float amount)
    {
        if(amount < 0)
        {
            Debug.LogWarning("Cannot add a negative amount of water.");
            return;
        }
        waterLevel += amount;
    }

    public void ConsumeWater(float amount)
    {
        if(amount < 0)
        {
            Debug.LogWarning("Cannot consume a negative amount of water.");
            return;
        }
        waterLevel = Mathf.Max(0, waterLevel - amount);
    }

    public void AddNutrients(float amount)
    {
        if(amount < 0)
        {
            Debug.LogWarning("Cannot add a negative amount of nutrients.");
            return;
        }
        nutrientLevel += amount;
    }

    public void ConsumeNutrients(float amount)
    {
        if(amount < 0)
        {
            Debug.LogWarning("Cannot consume a negative amount of nutrients.");
            return;
        }
        nutrientLevel = Mathf.Max(0, nutrientLevel - amount);
    }
}