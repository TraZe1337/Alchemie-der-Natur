using UnityEngine;

public class NegativeHealthEffectVisualizer : MonoBehaviour
{
    Renderer[] dehydrationRendererOfChildPlants;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void UpdateHealthEffectColor(PlantHeathStages stage, float value) {
        if (transform.childCount >= 1)
        {
            CeckForChildren();
        }
        else
        {
            return;
        }

        Color startColor = new Color(1f, 1f, 1f, 1f);              
        Color targetColor = new Color(1f, 1f, 1f, 1f);            


        switch (stage)
        {
            case PlantHeathStages.Dehydration:                
                targetColor = new Color(1f, 152f / 255f, 0f, 1f);           // (255,152,0,255)
                break;
            case PlantHeathStages.Overwatered:
                targetColor = new Color(84f / 255f, 84f / 255f, 84f / 255f, 255f);  
                break;
            case PlantHeathStages.InTheDark:
                // Show in the dark effect
                break;
            case PlantHeathStages.OverexposedToSunlight:
                // Show overexposed to sunlight effect
                break;
        }

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
