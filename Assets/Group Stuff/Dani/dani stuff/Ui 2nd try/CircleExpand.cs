using UnityEngine;

public class ExpandCollapse : MonoBehaviour
{
    [Header("Settings")]
    public float expandSize = 1.5f;         // Scale factor for expansion
    public float animationTime = 0.5f;     // Time for expand/collapse animation
    public float delayBetween = 0.2f;      // Delay between animations
    public float holdAtMinTime = 2.0f;     // Time to hold at the smallest size
    public float moveOffsetX = 0.5f;       // Distance to move right during expansion
    public float moveOffsetY = -0.5f;      // Distance to move down during expansion
    public float targetOpacity = 0.6f;     // Target opacity (alpha) during expansion

    private Vector3 originalScale;         // Original scale of the object
    private Vector3 originalPosition;      // Original position of the object
    private SpriteRenderer spriteRenderer; // SpriteRenderer for controlling opacity

    void Start()
    {
        // Save the original scale and position of the object
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;

        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start the animation loop
        StartExpandCollapse();
    }

    private void StartExpandCollapse()
    {
        // Target position for expanding
        Vector3 expandedPosition = originalPosition + new Vector3(moveOffsetX, moveOffsetY, 0);

        // Expand the object and move it with ease-in-out
        LeanTween.scale(gameObject, originalScale * expandSize, animationTime)
            .setEase(LeanTweenType.easeInOutQuad); // Ease-in-out for smooth scaling

        LeanTween.moveLocal(gameObject, expandedPosition, animationTime)
            .setEase(LeanTweenType.easeInOutQuad) // Ease-in-out for smooth movement
            .setOnComplete(() =>
            {
                // Gradually lower the opacity during expansion
                LeanTween.value(gameObject, UpdateOpacity, spriteRenderer.color.a, targetOpacity, animationTime)
                    .setEase(LeanTweenType.easeInOutQuad)
                    .setOnComplete(() =>
                    {
                        // Wait before collapsing
                        Invoke(nameof(StartCollapse), delayBetween);
                    });
            });
    }

    private void StartCollapse()
    {
        // Collapse the object and move it back with ease-in-out
        LeanTween.scale(gameObject, originalScale, animationTime)
            .setEase(LeanTweenType.easeInOutQuad); // Ease-in-out for smooth scaling

        LeanTween.moveLocal(gameObject, originalPosition, animationTime)
            .setEase(LeanTweenType.easeInOutQuad); // Ease-in-out for smooth movement

        // Gradually restore the opacity during collapse
        LeanTween.value(gameObject, UpdateOpacity, spriteRenderer.color.a, 1.0f, animationTime)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                // Hold at the smallest size
                Invoke(nameof(StartExpandCollapse), holdAtMinTime);
            });
    }

    private void UpdateOpacity(float newAlpha)
    {
        // Update the sprite's alpha value
        Color color = spriteRenderer.color;
        color.a = newAlpha;
        spriteRenderer.color = color;
    }
}
