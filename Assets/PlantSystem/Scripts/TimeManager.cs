using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // Optional: Using a static instance to easily access the TimeManager from elsewhere.
    public static TimeManager Instance;

    // List to hold all registered plant objects.
    private List<PlantGrowth> plantGrowths = new List<PlantGrowth>();

    void Awake()
    {
        // Singleton pattern setup. Only one TimeManager should exist.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        // Get the delta time (time between frames)
        float deltaTime = Time.deltaTime;

        // Update each registered plant's growth progress.
        foreach (PlantGrowth plantGrowth in plantGrowths)
        {
            plantGrowth.TickGrowth(deltaTime);
        }
    }

    // Method for plant objects to register themselves
    public void RegisterPlant(PlantGrowth plantGrowth)
    {
        Debug.Log("Registering plant: " + plantGrowth.gameObject.name);
        if (!plantGrowths.Contains(plantGrowth))
        {
            plantGrowths.Add(plantGrowth);
        }
    }

    // Method for plant objects to unregister (e.g., if they are destroyed)
    public void UnregisterPlant(PlantGrowth plantGrowth)
    {
        if (plantGrowths.Contains(plantGrowth))
        {
            plantGrowths.Remove(plantGrowth);
        }
    }
}
