using UnityEngine;

public class PlantSpawner : MonoBehaviour
{
    [SerializeField] private PlantSO plantSO;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNewPlantState(int state) {
        if (state < 0 || state >= plantSO.MaxStage)
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
