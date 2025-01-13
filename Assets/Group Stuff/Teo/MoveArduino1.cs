using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArduino1 : MonoBehaviour
{
    [SerializeField] private GameObject breathProject;       // Projectile prefab
    [SerializeField] private Transform breathTarget;         // Fire point
    [SerializeField] private float maxCharge = 5f;           // Maximum charge amount
    [SerializeField] private float projectSpeed = 5f;        // Speed of projectile
    [SerializeField] private ParticleSystem chargingParticles; // Particle system for charging effect

    private float chargeAmount = 0f;                         // Current charge amount
    private bool fireButtonPressed = false;                  // Is the fire button pressed
    private bool fireButtonHandled = false;                  // Has the fire button been handled
    private bool isCharged = false;                          // Is charging in progress

    void Start()
    {
        // Ensure particles do not play on start
        if (chargingParticles != null)
        {
            chargingParticles.Stop();
        }
    }

    void Update()
    {
        HandleInput(); // Check for keyboard input

        // Handle charging
        if (isCharged)
        {
            chargeAmount += Time.deltaTime;
            chargeAmount = Mathf.Clamp(chargeAmount, 0, maxCharge); // Cap charge amount

            float normalizedCharge = chargeAmount / maxCharge; // Normalize charge
            Debug.Log($"Charge Amount: {chargeAmount}, Normalized Charge: {normalizedCharge}");

            UpdateParticleEffects(normalizedCharge); // Update particle effects based on charge
        }
        else
        {
            if (chargingParticles.isPlaying)
            {
                chargingParticles.Stop(); // Stop particles when not charging
            }
        }

        // Handle firing
        if (fireButtonPressed && !fireButtonHandled)
        {
            FireProjectile();
            fireButtonHandled = true; // Mark fire as handled
        }
    }

    private void HandleInput()
    {
        // Start charging on 'E' key
        if (Input.GetKey(KeyCode.E))
        {
            isCharged = true;
        }
        else if (!Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Q))
        {
            // Stop charging and reset firing when no keys are pressed
            isCharged = false;
            fireButtonPressed = false;
            fireButtonHandled = false;
        }

        // Fire projectile on 'Q' key
        if (Input.GetKey(KeyCode.Q))
        {
            fireButtonPressed = true;
        }
    }

    private void FireProjectile()
    {
        Debug.Log("FireProjectile called");

        if (chargeAmount > 0)
        {
            // Instantiate the projectile
            GameObject projectile = Instantiate(breathProject, breathTarget.position, breathTarget.rotation);
            Debug.Log("Projectile instantiated");

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Set projectile velocity based on charge  
                float force = projectSpeed * chargeAmount;
                rb.linearVelocity = breathTarget.forward * force; // Fixed typo: replaced `linearVelocity` with `velocity`
            }

            chargeAmount = 0f; // Reset charge after firing
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

        // Update particle emission rate and color based on charge
        var emissionModule = chargingParticles.emission;
        emissionModule.rateOverTime = Mathf.Lerp(10, 100, normalizedCharge); // Increase particle emission with charge

        var mainModule = chargingParticles.main;
        mainModule.startSize = Mathf.Lerp(0.1f, 1f, normalizedCharge); // Adjust particle size
        mainModule.startColor = Color.Lerp(Color.blue, Color.red, normalizedCharge); // Change color from blue to red
    }
}