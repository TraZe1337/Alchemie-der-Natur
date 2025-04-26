using System;
using UnityEngine;

public class PlantPreviewUI : MonoBehaviour
{
    [SerializeField] private GameObject PlantPreviewUIDocument;
    [SerializeField] private Pot pot;
    private bool playerInRange = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (PlantPreviewUIDocument.activeSelf)
            {
                PlantPreviewUIDocument.SetActive(false);
            }
            else
            {
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
