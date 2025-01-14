using UnityEngine;

public class SmoothedYAxisScript : MonoBehaviour
{
    [SerializeField] private Calibration calibrationScript;
    [SerializeField] private GameObject breathProject;
    [SerializeField] private Transform breathTarget;
    [SerializeField] private float maxCharge = 5f;
    [SerializeField] private float projectSpeed = 5f;
    [SerializeField] private ParticleSystem chargingParticles;
    [SerializeField] private float smoothingFactor = 0.1f; // Adjust for sensitivity

    private float chargeAmount = 0f;
    private bool isCharging = false;
    private float smoothedY = 0f;

    void Update()
    {
        if (!calibrationScript.IsCalibrated) return;

        float relativeY = calibrationScript.RelativeYPosition;

        // Smooth the Y-axis value
        smoothedY = Mathf.Lerp(smoothedY, relativeY, smoothingFactor);

        Debug.Log($"Relative Y: {relativeY}, Smoothed Y: {smoothedY}");

        if (smoothedY >= 0.003f) // Adjust the threshold based on the smoothed value
        {
            StartCharging();
        }
        else if (smoothedY <= 0.002999f && isCharging)
        {
            FireProjectile();
        }
        else
        {
            StopCharging();
        }
    }

    private void StartCharging()
    {
        isCharging = true;
        chargeAmount += Time.deltaTime;
        chargeAmount = Mathf.Clamp(chargeAmount, 0, maxCharge);

        float normalizedCharge = chargeAmount / maxCharge;
        Debug.Log($"Charging... Charge Amount: {chargeAmount}, Normalized Charge: {normalizedCharge}");

        UpdateParticleEffects(normalizedCharge);
    }

    private void StopCharging()
    {
        isCharging = false;
        chargeAmount = 0f;

        if (chargingParticles.isPlaying)
        {
            chargingParticles.Stop();
        }
    }

    private void FireProjectile()
    {
        Debug.Log("Firing projectile!");

        if (chargeAmount > 0.1f)
        {
            GameObject projectile = Instantiate(breathProject, breathTarget.position, breathTarget.rotation);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float force = projectSpeed * chargeAmount;
                rb.linearVelocity = breathTarget.forward * force; // Corrected from `linearVelocity` to `velocity`
            }

            chargeAmount = 0f; // Reset charge after firing
            StopCharging();
        }
        else
        {
            Debug.Log("Charge amount is zero, no projectile fired");
        }
    }

    private void UpdateParticleEffects(float normalizedCharge)
    {
        if (!chargingParticles.isPlaying)
        {
            chargingParticles.Play();
        }

        var emissionModule = chargingParticles.emission;
        emissionModule.rateOverTime = Mathf.Lerp(10, 100, normalizedCharge);

        var mainModule = chargingParticles.main;
        mainModule.startSize = Mathf.Lerp(0.1f, 1f, normalizedCharge);
        mainModule.startColor = Color.Lerp(Color.blue, Color.red, normalizedCharge);
    }
}