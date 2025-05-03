using UnityEngine;
using System;

public class Pot : MonoBehaviour, IUsable
{
    [SerializeField] private float waterLevel = 100f;
    [SerializeField] private readonly float maxWaterLevel = 200f;
    [SerializeField] private float sunlightLevel = 1f;
    [SerializeField] private float nutrientLevel = 50f;
    [SerializeField] private readonly float maxNutrientsLevel = 200f;
    [SerializeField] private GameObject healthStatusUI; // Reference to the HealthStatusUI script
    [SerializeField] private PlantSO myPotPlant; // TEMPORARY | TODO: The plant will be added dynamically via the use by planting a plant (not hardcoded via the editor)
    [SerializeField] private PlantGrowth myPlant; // TEMPORARY | TODO: The plant will be added dynamically via the use by planting a plant (not hardcoded via the editor)
    private bool _playerInRange = false;
    public float CurrentWaterLevel => waterLevel;
    public float MaxWaterLevel => maxWaterLevel;
    public float MaxNutrientsLevel => maxNutrientsLevel;
    public float CurrentSunlightLevel => sunlightLevel;
    public float CurrentNutrientLevel => nutrientLevel;
    public String PlantName => myPotPlant.plantName;
    public String PlantDescription => myPotPlant.plantDescription;
    public event Action<bool> OnPlayerInRangeChanged;

    private bool PlayerInRange
    {
        get => _playerInRange;
        set
        {
            if (_playerInRange != value)
            {
                _playerInRange = value;
                OnPlayerInRangeChanged?.Invoke(_playerInRange);
            }
        }
    }
    void Update()
    {
        //UpdateEnvironment(Time.deltaTime);
    }

    public void UpdateEnvironment(float deltaTime)
    {
        // Example: water evaporation over time
        waterLevel = Mathf.Max(0, waterLevel - 0.5f * deltaTime);
    }

    public void SetPlant(PlantSO plantType)
    {
        // TODO: Implement logic to set the plant in the pot (Spawn GameObject, etc.)
    }

    public void AddWater(float amount)
    {
        if(amount < 0)
        {
            Debug.LogWarning("Cannot add a negative amount of water.");
            return;
        }
        waterLevel = Mathf.Min(waterLevel + amount, maxWaterLevel);
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
        nutrientLevel = Mathf.Min(nutrientLevel + amount, maxNutrientsLevel);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;
            healthStatusUI.SetActive(true);
            Debug.Log("Player entered the pot area.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = false;
            healthStatusUI.SetActive(false);
            Debug.Log("Player exited the pot area.");
        }
    }

    public int HarvestPotPlant()
    {
        if (myPotPlant == null)
        {
            Debug.LogWarning("No plant in the pot to harvest.");
            return 0;
        }
        Debug.Log($"Harvesting {myPlant} from the pot.");
        
        try {
            return myPlant.Harvest(); 
        } catch (Exception e) {
            Debug.Log($"Normal while harvesting plant (plant already harvested because method called each frame): {e.Message}");
            return 0;
        }
    }
}