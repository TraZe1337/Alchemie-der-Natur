using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class PlantButtonUI : MonoBehaviour
{
    public Transform mainCamera;
    [SerializeField] private Pot pot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get Main Camera transform
        mainCamera = Camera.main?.transform; // Safely get the main camera's transform
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found. Please ensure a camera is tagged as 'MainCamera'.");
        }
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P key pressed, should Plant a new plant in the pot.");
        }
    }

    void LateUpdate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - mainCamera.position);
        transform.rotation = targetRotation;
    }

}
