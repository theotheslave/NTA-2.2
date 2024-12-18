using UnityEngine;
using System.Collections.Generic;
public class BallCentre : MonoBehaviour
{
      [SerializeField] private Transform targetCenter;       // The center target
    [SerializeField] private float moveSpeed = 5f;         // Speed of movement toward the center
    [SerializeField] private float maxScale = 2f;          // Maximum scale for growth
    [SerializeField] private float minScale = 0.5f;        // Minimum scale for shrinkage
    [SerializeField] private float growSpeed = 1f;         // Speed of growth/shrinkage
    [SerializeField] private float holdTime = 4f;          // Hold time at max or min scale
    [SerializeField] private List<GameObject> objectsToGrow; // Specific objects to grow

    private bool isMovingToCenter = false;                 // Is the object moving to the center?
    private bool hasReachedCenter = false;                 // Has the object reached the center?
    private Vector3 originalScale;                         // Original scale of the object
    private bool isGrowing = true;                         // Whether the object is growing
    private float holdTimer = 0f;                          // Timer for hold duration
    private bool isHolding = false;                        // Whether the object is holding its scale

    void Start()
    {
        originalScale = transform.localScale; // Store the original scale
    }

    void Update()
    {
        if (isMovingToCenter && !hasReachedCenter)
        {
            Debug.Log("Moving toward the center...");
            // Move the object toward the center
            transform.position = Vector3.MoveTowards(transform.position, targetCenter.position, moveSpeed * Time.deltaTime);

            // Check if the object has reached the center
            if (Vector3.Distance(transform.position, targetCenter.position) < 0.1f)
            {
                Debug.Log("Reached the center!");
                hasReachedCenter = true; // Mark as reached
                isMovingToCenter = false; // Stop moving
                TriggerGrowthOnSpecificObjects(); // Trigger specific objects to grow
            }
        }

        if (hasReachedCenter)
        {
            Debug.Log("Handling scaling...");
            HandleScaling(); // Handle grow/shrink behavior for the center object
        }
    }

    private void HandleScaling()
    {
        if (isHolding)
        {
            // Countdown the hold timer
            holdTimer -= Time.deltaTime;
            if (holdTimer <= 0f)
            {
                // End holding and switch states
                isHolding = false;
                isGrowing = !isGrowing;
            }
            return; // Skip the scaling logic while holding
        }

        // Handle growing or shrinking
        Vector3 targetScale = isGrowing ? originalScale * maxScale : originalScale * minScale;
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, growSpeed * Time.deltaTime);

        // Check if it has reached the target scale
        if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
        {
            transform.localScale = targetScale; // Snap to the target scale
            isHolding = true; // Start holding
            holdTimer = holdTime; // Reset hold timer
        }
    }

    private void TriggerGrowthOnSpecificObjects()
    {
        foreach (GameObject obj in objectsToGrow)
        {
            if (obj != null)
            {
                GrowAndShrink growScript = obj.GetComponent<GrowAndShrink>();
                if (growScript != null)
                {
                    // Optionally enable the script or trigger some method to start its growth
                    growScript.enabled = true;
                }
            }
        }

        Debug.Log($"Triggered growth for {objectsToGrow.Count} objects.");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collided with: {other.name}");
        if (other.CompareTag("Place")) // Trigger the movement to the center
        {
            Debug.Log("Trigger activated!");
            isMovingToCenter = true; // Start moving toward the center
        }
    }
}
