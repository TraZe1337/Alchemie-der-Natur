using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class DehydrationColorChanger : MonoBehaviour
{


    Renderer[] dehydrationRendererOfChildPlants;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public void UpdateDehydrationColor(float value)
    {
        if (transform.childCount >= 1)
        {
            CeckForChildren();
        }
        else
        {
            return;
        }

        //Debug.Log("Dehydration Value: " + value + " will set new color");
        if (value == 1)
        {
            // If the value is 1, set the color to the target color directly
            foreach (Renderer rend in dehydrationRendererOfChildPlants)
            {
                rend.material.color = new Color(185f / 255f, 122f / 255f, 0f, 255f); // (185,122,0,255)
            }
            return;
        }
        // Clamp value between 0 and 1 in case the input is out of range
        value = Mathf.Clamp01(value);

        // Define the starting color (white) and the target color (255,152,0,255)
        Color startColor = new Color(1f, 1f, 1f, 1f);                  // (255,255,255,255)
        Color targetColor = new Color(1f, 152f / 255f, 0f, 1f);           // (255,152,0,255)

        // Interpolate between the two colors based on the given value
        Color newColor = Color.Lerp(startColor, targetColor, value);

        // Apply the new color to the material of the dehydrationRenderer
        foreach (Renderer rend in dehydrationRendererOfChildPlants)
        {
            rend.material.color = newColor;
        }
    }

    private void CeckForChildren()
    {
        dehydrationRendererOfChildPlants = GetComponentsInChildren<Renderer>();

    }
}
