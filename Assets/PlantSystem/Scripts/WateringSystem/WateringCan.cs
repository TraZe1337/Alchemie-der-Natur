using UnityEngine;

public class WateringCan : MonoBehaviour
{
    [SerializeField] private float currentWaterAmount = 100f; // Amount of water in the can
    [SerializeField] private float maxWaterAmount = 100f; // Maximum capacity of the watering can
    [SerializeField] private float waterUsageRate = 1f; // Amount of water used per interaction
    [SerializeField] private ParticleSystem sprayFX;

    void Start()
    {
        sprayFX?.Stop();
    }

    public void StopWatering()
    {
        sprayFX?.Stop();
    }

    public float DispenseWater(float deltaTime)
    {
        if (sprayFX != null && !sprayFX.isPlaying)
        {
            sprayFX.Play();
            Debug.Log("sprayFX started.");
        }
        float used = waterUsageRate * deltaTime;
        used = Mathf.Min(used, currentWaterAmount);
        currentWaterAmount -= used;
        if (currentWaterAmount <= 0f)
        {
            currentWaterAmount = 0f;
            StopWatering();
        }
        return used;
    }
}
