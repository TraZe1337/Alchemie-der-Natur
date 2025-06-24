using UnityEngine;
using System.Collections.Generic;

public class AdNPlayerInventory : MonoBehaviour
{
    private List<GameObject> _items = new List<GameObject>();

    public void AddItem(GameObject item)
    {
        _items.Add(item);
        // Customize: update UI, stats, etc.
        Debug.Log(item.name + " added to inventory.");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
