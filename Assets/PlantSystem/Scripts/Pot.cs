using UnityEngine;
using System;

public class Pot : MonoBehaviour
{
    [SerializeField] private float waterLevel = 100f;
    [SerializeField] private float sunlightLevel = 1f;
    [SerializeField] private float nutrientLevel = 50f;
    [SerializeField] private GameObject healthStatusUI; // Reference to the HealthStatusUI script
    private bool _playerInRange = false;
    public float CurrentWaterLevel => waterLevel;
    public float CurrentSunlightLevel => sunlightLevel;
    public float CurrentNutrientLevel => nutrientLevel;
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
}