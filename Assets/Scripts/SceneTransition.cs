using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    [Header("Teleport Settings")]
    [Tooltip("Transform representing the destination position and rotation")]  
    public Transform teleportTarget;

    [Tooltip("Tag used to identify the player object")]  
    public string playerTag = "Player";

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called.");
        if (other.CompareTag(playerTag))
        {
            if (teleportTarget != null)
            {
                Debug.Log($"Teleporting to position: {teleportTarget.position}, rotation: {teleportTarget.rotation}");

                Rigidbody rb = other.GetComponent<Rigidbody>();
                Collider col = other.GetComponent<Collider>();

                if (rb != null)
                {
                    rb.isKinematic = true; // Temporarily disable physics
                }

                if (col != null)
                {
                    col.enabled = false; // Temporarily disable collider
                }

                other.transform.position = teleportTarget.position;
                other.transform.rotation = teleportTarget.rotation;

                Physics.SyncTransforms(); // Force physics engine to recognize the change

                if (col != null)
                {
                    col.enabled = true; // Re-enable collider
                }

                if (rb != null)
                {
                    rb.isKinematic = false; // Re-enable physics
                }
            }
            else
            {
                Debug.LogError("Teleport target is not assigned!");
            }
        }
    }


}
