using UnityEngine;

public class ToggleParticleOnTrigger : MonoBehaviour
{
    public ParticleSystem particleStuff;

    private bool isParticlePlaying = false;

    public float triggerRadius = 5f;

    public Transform player;

    void Start()
    {
        if (particleStuff == null)
        {
            particleStuff = GetComponent<ParticleSystem>();
        }

        if (particleStuff != null && particleStuff.isPlaying)
        {
            particleStuff.Stop();
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
        if (particleStuff != null)
        {
            particleStuff.Play();
            isParticlePlaying = true;
            Debug.Log("Particle system started.");
        }
    }

    private void StopParticles()
    {
        if (particleStuff != null)
        {
            particleStuff.Stop();
            isParticlePlaying = false;
            Debug.Log("Particle system stopped.");
        }
    }
}
