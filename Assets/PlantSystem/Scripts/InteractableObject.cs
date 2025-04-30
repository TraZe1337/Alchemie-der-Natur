using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string objectName;
    public bool canBePickedUp = true;
    private Rigidbody rb;
    private bool isPickedUp = false;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    public void Interact(string interactionType)
    {
        Debug.Log($"{objectName} interacted with using {interactionType}");

        // Example logic for "Primary" or "Secondary"
        if (interactionType == "Primary")
        {
            // Do something primary
        }
        else if (interactionType == "Secondary")
        {
            // Something else
        }
    }
}
