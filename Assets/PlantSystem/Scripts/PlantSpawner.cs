using UnityEngine;
using RedstoneinventeGameStudio;

public class PlantSpawner : MonoBehaviour
{
    [SerializeField] private PlantSO plantSO;
    [SerializeField] private CardManager plantSlot; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawn a new plant state based on the current growth stage
    public void SpawnNewPlantState(int state) {
        if (state < 0 || state >= plantSO.MaxGrowthStage)
        {
            Debug.LogError("Invalid plant state: " + state);
            return;
        }

        GameObject plantPrefab = plantSO.GetPlantPrefab(state);
        if (plantPrefab != null)
        {
            GameObject plantInstance = Instantiate(plantPrefab, transform);
            plantInstance.transform.SetParent(transform);
        }
        else
        {
            Debug.LogError("No prefab found for state: " + state);
        }
    }
}
