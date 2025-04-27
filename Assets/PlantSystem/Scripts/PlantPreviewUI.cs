using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlantPreviewUI : MonoBehaviour
{
    [SerializeField] private GameObject PlantPreviewUIDocument;
    [SerializeField] private GameObject plantPreviewCamera;
    [SerializeField] private Pot pot;

    private bool playerInRange = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       plantPreviewCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.I))
        {
            if (PlantPreviewUIDocument.activeSelf)
            {
                plantPreviewCamera.SetActive(false);
                PlantPreviewUIDocument.SetActive(false);
            }
            else
            {
                plantPreviewCamera.SetActive(true);
                PlantPreviewUIDocument.SetActive(true);
            }
        }
    }
    private void HandlePlayerInRangeChanged(bool obj)
    {
        playerInRange = obj;
    }

    private void OnEnable()
    {
        if (pot != null)
            pot.OnPlayerInRangeChanged += HandlePlayerInRangeChanged;
    }

    private void OnDisable()
    {
        if (pot != null)
            pot.OnPlayerInRangeChanged -= HandlePlayerInRangeChanged;
    }
}
