using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class HealthStatusUI : MonoBehaviour
{
    public Transform mainCamera;
    public List<Image> images;
    private List<EffectSO> negativeHealthEffects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        negativeHealthEffects = new List<EffectSO>();
        // Get Main Camera transform
        mainCamera = Camera.main?.transform; // Safely get the main camera's transform
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found. Please ensure a camera is tagged as 'MainCamera'.");
        }
        ClearImages();
    }

    public void AddEffect(List<EffectSO> effects)
    {
        Debug.Log("EffectsMatch: " + EffectsMatch(effects));
        if (!EffectsMatch(effects))
        {
            Debug.Log("Effects do not match, updating UI with " + effects.Count + " effects.");
            negativeHealthEffects = new List<EffectSO>(effects);
            RestockEffectImages();
        }
    }
    
    public void ClearEffects()
    {
        ClearImages();
        negativeHealthEffects.Clear();
    }

    private void ClearImages()
    {
        foreach (var image in images)
        {
            image.enabled = false;
        }
    }

    private void RestockEffectImages() {
        // Clear existing images
        ClearImages();
        for (int i = 0; i < negativeHealthEffects.Count; i++)
        {
            if (negativeHealthEffects[i] == null)
                continue; // Skip null effects
            
            // Check if the effect image is not null or empty
            if (negativeHealthEffects[i].effectImage != null)
            {
                images[i].sprite = negativeHealthEffects[i].effectImage;
                images[i].enabled = true;
            }
        }
    }

    private bool EffectsMatch(List<EffectSO> effects)
    {
        if (effects == null || negativeHealthEffects == null)
            return false;
        
        if (effects.Count != negativeHealthEffects.Count)
            return false;
        
        // Create a HashSet for effect types from the parameter list.
        HashSet<PlantHealthStages> effectTypes = new HashSet<PlantHealthStages>();
        foreach (var effect in effects)
        {
            effectTypes.Add(effect.effectType);
        }
        
        // Create a HashSet for effect types from the negativeHealthEffects list.
        HashSet<PlantHealthStages> uiEffectTypes = new HashSet<PlantHealthStages>();
        foreach (var effect in negativeHealthEffects)
        {
            uiEffectTypes.Add(effect.effectType);
        }
        
        return effectTypes.SetEquals(uiEffectTypes);
    }

    

    void LateUpdate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - mainCamera.position);
        transform.rotation = targetRotation;
    }
}
