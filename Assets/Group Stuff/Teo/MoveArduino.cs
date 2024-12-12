using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class MoveArduino : MonoBehaviour
{
    [SerializeField] private GameObject breathProject;
    [SerializeField] private Transform breathTarget;
    [SerializeField] private float maxCharge = 5f;
    [SerializeField] private float projectSpeed = 10f;
    [SerializeField] private ParticleSystem chargingParticles;
    

    private float chargeAmount = 0f;
    private bool fireButtonPressed = false;
    private bool fireButtonHandled = false;
    private bool isCharged = false;

    SerialPort sp = new SerialPort("COM3", 9600); 

    public object Message { get; private set; }

    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 1;
    }

    void Update()
    {
        ReadArduinoInput();
        if (isCharged)
        {
            chargeAmount += Time.deltaTime;
            chargeAmount = Mathf.Clamp(chargeAmount, 0, maxCharge); 

            
            UpdateParticleEffects(chargeAmount / maxCharge); 
        }
        else
        {
            
            
            chargingParticles.Stop();
        }

        // if (isCharged && !fireButtonPressed)
        // {
        //   chargeAmount += Time.deltaTime;
        //   chargeAmount = Mathf.Clamp(chargeAmount, 0, maxCharge);
        //  Debug.Log($"Charge amount: {chargeAmount}");
        // }

        
        if (fireButtonPressed && !fireButtonHandled)
        {
            FireProject();
            fireButtonHandled = true; 
        }
    }

    void ReadArduinoInput()
    {
        if (sp.IsOpen)
        {

            try
            {
                int signal = sp.ReadByte(); 
                Debug.Log($"Signal received: {signal}");
                switch (signal)
                {
                    case 1: 
                        isCharged = true;
                        Debug.Log("Charging started");
                        break;
                    case 0: 
                        isCharged = false;
                        fireButtonPressed = false;
                        fireButtonHandled = false;
                        Debug.Log("Charging stopped, ready to fire again");
                        break;
                    case 2: 
                        fireButtonPressed = true;
                        Debug.Log("Fire button pressed");
                        break;
                }
            }
            catch (System.Exception)
            {
                
            }
        }
    }

    void FireProject()
    {
        Debug.Log("FireProjectile called");
        if (chargeAmount > 0)
        {
            GameObject projectile = Instantiate(breathProject, breathTarget.position, breathTarget.rotation);
            Debug.Log("Projectile instantiated");
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float force = projectSpeed * chargeAmount;
                rb.linearVelocity = breathTarget.forward * force; 
                Debug.Log($"Projectile velocity set to {rb.linearVelocity}");
            }

            chargeAmount = 0f; 
        }
        else
        {
            Debug.Log("Charge amount is zero, no projectile fired");
        }
    }
    void UpdateParticleEffects(float normalizedCharge)
    {
        if (!chargingParticles.isPlaying)
        {
            chargingParticles.Play();
        }

        
        var emissionModule = chargingParticles.emission;
        emissionModule.rateOverTime = Mathf.Lerp(10, 100, normalizedCharge); 

        var mainModule = chargingParticles.main;
        
        mainModule.startColor = Color.Lerp(Color.blue, Color.red, normalizedCharge); 
    }
}
