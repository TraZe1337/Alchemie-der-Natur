using UnityEngine;
using RedstoneinventeGameStudio;
using UnityEngine.UIElements;

public class PlantSpawner : MonoBehaviour
{
    [SerializeField] private Pot pot;
    [SerializeField] private HealthStatusUI healthStatusUI;
    [SerializeField] private CardManager cardManager;
    [SerializeField] private Transform plantRoot;
    [SerializeField] private GameObject plantObjectPrefab;
    [SerializeField] private PlantButtonUI plantButtonUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Spawn a new plant state based on the current growth stage
    public void SpawnNewPlantState()
    {
        if (cardManager.itemData == null)
        {
            Debug.LogError("PlantSO is not assigned!");
            return;
        }

        if (plantObjectPrefab != null)
        {
            GameObject plantInstance = Instantiate(plantObjectPrefab, plantRoot.transform);
            plantInstance.transform.SetParent(plantRoot);

            plantInstance.transform.localPosition = Vector3.zero;
            plantInstance.transform.localRotation = Quaternion.identity;

            // Set the plant's data
            PlantGrowth plantGrowthScript = plantInstance.GetComponent<PlantGrowth>();

            plantGrowthScript.plantData = cardManager.itemData.plantData;
            plantGrowthScript.pot = pot;
            plantGrowthScript.healthStatusUI = healthStatusUI;

            // Activate the plant instance and trigger the registering process in the time manager
            pot.SetInSemen(true);
            plantInstance.SetActive(true);
            cardManager.UnSetItem();
            plantButtonUI.CloseUI();
        }
        else
        {
            Debug.LogError("No prefab found");
            return;
        }
    }
}
