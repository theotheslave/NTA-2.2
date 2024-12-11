using UnityEngine;

public class ResizeOnApproach : MonoBehaviour
{
    // The amount by which the object will scale when the player enters the trigger
    public Vector3 scaleMultiplier = new Vector3(2f, 2f, 2f);
    private Vector3 originalScale;
    
    // Optionally, set the distance at which the scaling should happen
    public float triggerDistance = 5f;

    private void Start()
    {
        // Store the original scale of the object
        originalScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player (or specific object) enters the trigger zone
        if (other.CompareTag("Player"))
        {
            // Increase the size of the object
            transform.localScale = originalScale * scaleMultiplier.x; // You can choose a uniform or customized scaling
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Optionally, revert the object to its original size when the player exits the trigger zone
        if (other.CompareTag("Player"))
        {
            transform.localScale = originalScale;
        }
    }

    private void Update()
    {
        // Optionally, check if player is within a specific range and increase size gradually if needed
        if (Vector3.Distance(transform.position, Camera.main.transform.position) <= triggerDistance)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * scaleMultiplier.x, Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime);
        }
    }
}
