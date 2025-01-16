using UnityEngine;

public class ToggleParticleOnTrigger : MonoBehaviour
{
    public ParticleSystem particleSystem;

    private bool isParticlePlaying = false;

    public float triggerRadius = 5f;

    public Transform player;

    void Start()
    {
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        if (particleSystem != null && particleSystem.isPlaying)
        {
            particleSystem.Stop();
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= triggerRadius && !isParticlePlaying)
            {
                PlayParticles();
            }
            else if (distance > triggerRadius && isParticlePlaying)
            {
                StopParticles();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isParticlePlaying)
            {
                PlayParticles();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isParticlePlaying)
            {
                StopParticles();
            }
        }
    }

    private void PlayParticles()
    {
        if (particleSystem != null)
        {
            particleSystem.Play();
            isParticlePlaying = true;
            Debug.Log("Particle system started.");
        }
    }

    private void StopParticles()
    {
        if (particleSystem != null)
        {
            particleSystem.Stop();
            isParticlePlaying = false;
            Debug.Log("Particle system stopped.");
        }
    }
}
