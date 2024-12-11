using UnityEngine;

public class GrowAndShrink : MonoBehaviour
{
    public float growSpeed = 1f; // Speed of growth
    public float maxScale = 2f; // Maximum scale
    public float minScale = 0.5f; // Minimum scale
    public float holdTime = 4f; // Time to hold at max or min scale

    private Vector3 originalScale; // Original scale of the ball
    private bool isGrowing = true; // Whether the ball is growing
    private float holdTimer = 0f; // Timer for hold duration
    private bool isHolding = false; // Whether the ball is holding its scale

    void Start()
    {
        // Store the original scale
        originalScale = transform.localScale;
    }

    void Update()
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
}
