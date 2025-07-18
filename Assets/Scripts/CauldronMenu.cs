using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class CauldronMenu : MonoBehaviour
{
    public GameObject openUi_button; // The UI button that appears
    public GameObject closeUi_button; // The UI button that appears
    public GameObject canvas; // The canvas that opens when the button is pressed
    public GameObject inventoryCanvas; // The canvas that opens when the button is pressed
    public GameObject MainCam;
    public GameObject CauldronCam; // The camera used in alchemy mode
    public Transform playerPos; // Position of player in alchemy mode

    GameObject player; // The player
    private bool isInRange = false; // Flag to check if player is in trigger area
    private GameObject crosshair; // Reference to the crosshair GameObject

    void Start()
    {
        openUi_button.SetActive(false);
        closeUi_button.SetActive(false);

        canvas.SetActive(false);
        Debug.Log("Canvas hidden at start.");

        player = GameObject.FindGameObjectWithTag("Player");

        MainCam = GameObject.FindGameObjectWithTag("MainCamera");

        CauldronCam = GameObject.FindGameObjectWithTag("CauldronCam");

        crosshair = GameObject.FindGameObjectWithTag("Crosshair");
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player pressed 'E' while in range.");

            if (canvas.activeSelf)
            {
                Debug.Log("Canvas is active. Closing it now.");
                CloseCanvas();
                openUi_button.SetActive(true);
                closeUi_button.SetActive(false);
            }
            else
            {
                Debug.Log("Canvas is not active. Opening it now.");
                OpenCanvas();
                openUi_button.SetActive(false);
                closeUi_button.SetActive(true);
            }
        }
    }

    void OpenCanvas()
    {
        canvas.SetActive(true);
        Debug.Log("Canvas opened.");

        OpenInventory(); // Open the inventory when the cauldron is opened

        CauldronCam.GetComponent<Camera>().enabled = true;
        MainCam.GetComponent<Camera>().enabled = false;
        Debug.Log("Switched to CauldronCam.");


        // player.GetComponent<FirstPersonController>().LockCameraPosition = true;
        player.GetComponent<StarterAssetsInputs>().cursorInputForLook = false;
        player.GetComponent<StarterAssetsInputs>().cursorLocked = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor

        // Disable player movement
        player.GetComponent<FirstPersonController>().enabled = false;
        player.GetComponent<StarterAssetsInputs>().move = Vector2.zero; // Stop movement

        // Stop walking animation
        Animator animator = player.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetFloat("Speed", 0f); // Assuming "Speed" controls the walking animation
        }

        player.transform.position = playerPos.position;
        player.transform.rotation = Quaternion.Euler(0, 75, 0); // Adjust rotation as needed
        Debug.Log("Player moved to alchemy position.");

        crosshair.SetActive(false); // Hide the crosshair when the canvas is open
    }

    void CloseCanvas()
    {
        canvas.SetActive(false);
        Debug.Log("Canvas closed.");

        CloseInventory(); // Close the inventory if it's open

        CauldronCam.GetComponent<Camera>().enabled = false;
        MainCam.GetComponent<Camera>().enabled = true;
        Debug.Log("Switched back to MainCam.");

        // player.GetComponent<FirstPersonController>().LockCameraPosition = false;
        player.GetComponent<StarterAssetsInputs>().cursorInputForLook = true;
        player.GetComponent<StarterAssetsInputs>().cursorLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Re-enable player movement
        player.GetComponent<FirstPersonController>().enabled = true;
        player.GetComponent<StarterAssetsInputs>().move = Vector2.zero; // Reset movement

        crosshair.SetActive(true); // Show the crosshair when the canvas is closed
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            openUi_button.SetActive(true);
            Debug.Log("Player entered range. Interaction button shown.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            openUi_button.SetActive(false);
            closeUi_button.SetActive(false);
            Debug.Log("Player exited range. Interaction button hidden.");
        }
    }

    void OnDrawGizmos()
    {
        if (playerPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(playerPos.position, 0.1f);
        }
    }
}
