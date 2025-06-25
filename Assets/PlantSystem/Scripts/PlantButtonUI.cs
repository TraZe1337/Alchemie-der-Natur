using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class PlantButtonUI : MonoBehaviour
{
    public Transform mainCamera;
    [SerializeField] private Pot pot;
    [SerializeField] private GameObject PlantToPlantUI;
    private GameObject inventoryCanvas; // The canvas that opens when the button is pressed
                                        // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    { 
        inventoryCanvas = GameObject.FindGameObjectWithTag("InventoryCanvas");
        if (inventoryCanvas == null)
        {
            Debug.LogError("Inventory Canvas not found. Please ensure a canvas is tagged as 'InventoryCanvas'.");
        }
    }


    void Start()
    {
        // Get Main Camera transform
        mainCamera = Camera.main?.transform; // Safely get the main camera's transform
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found. Please ensure a camera is tagged as 'MainCamera'.");
        }
        LateStart();
    }

    void LateStart()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (PlantToPlantUI.activeSelf)
            {
                CloseUI();
            }
            else
            {
                PlantToPlantUI.SetActive(true);
                OpenInventory();
                UnityEngine.Cursor.visible = true;
                UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.None;
            }
        }
    }

    void LateUpdate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - mainCamera.position);
        transform.rotation = targetRotation;
    }

    void OpenInventory()
    {
        inventoryCanvas.SetActive(true);
        Debug.Log("Canvas opened.");

    }

    void CloseInventory()
    {
        inventoryCanvas.SetActive(false);
        Debug.Log("Canvas closed.");

    }

    private void OnDisable()
    {
        CloseUI();
    }


    public void CloseUI()
    { 
        PlantToPlantUI.SetActive(false);
        CloseInventory();
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.Locked;
    }


}
