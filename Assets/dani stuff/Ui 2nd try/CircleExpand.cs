using UnityEngine;

public class CircleExpand : MonoBehaviour
{
    [Header("Settings")]
    public float expandSize = 1.5f; // Scale factor for expansion
    public float animationTime = 0.5f; // Time for each animation phase
    public float delayBetween = 0.2f; // Delay between expand and collapse
    public float holdAtMinTime = 0.3f; // Time to hold at the smallest size
    private Vector3 originalScale;

    void Start()
    {
        // Save the original scale of the object
        originalScale = transform.localScale;

        // Start the expand and collapse loop
        StartExpandCollapse();
    }

    private void StartExpandCollapse()
    {
        // Expand the object
        LeanTween.scale(gameObject, originalScale * expandSize, animationTime)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() =>
            {
                // Wait for a delay, then collapse
                LeanTween.scale(gameObject, originalScale, animationTime)
                    .setEase(LeanTweenType.easeInOutSine)
                    .setDelay(delayBetween)
                    .setOnComplete(() =>
                    {
                        // Hold at the smallest size
                        Invoke(nameof(StartExpandCollapse), holdAtMinTime);
                    });
            });
    }
}
