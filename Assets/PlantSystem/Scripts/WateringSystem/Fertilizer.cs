using UnityEngine;

public class Fertilizer : MonoBehaviour
{
    [SerializeField] private float currentNutrientAmount = 100f; // Amount of water in the can
    [SerializeField] private float maxNutrientAmount = 100f; // Maximum capacity of the watering can
    [SerializeField] private float nutrientUsageRate = 1f; // Amount of water used per interaction
    [SerializeField] private ParticleSystem sprayFX; 
    void Start()
    {
        sprayFX?.Stop();
    }

    public void StopFertilizing()
    {
        sprayFX?.Stop();
    }

    public float DispenseFertilizer(float deltaTime)
    {
        if (sprayFX != null && !sprayFX.isPlaying)
        {
            sprayFX.Play();
            Debug.Log("sprayFX started.");
        }
        float used = nutrientUsageRate * deltaTime;
        used = Mathf.Min(used, currentNutrientAmount);
        currentNutrientAmount -= used;
        if (currentNutrientAmount <= 0f)
        {
            currentNutrientAmount = 0f;
            StopFertilizing();
        }
        return used;
    }
}
