using UnityEngine;

public class CircleAnimation : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float expandSize = 1.5f;         // Scale factor for expansion
    [SerializeField] private float animationTime = 0.5f;     // Time for expand/collapse animation
    [SerializeField] private float delayBetween = 0.2f;      // Delay between animations
    [SerializeField] private float holdAtMaxTime = 2.0f;     // Time to hold at the largest size
    [SerializeField] private float holdAtMinTime = 2.0f;     // Time to hold at the smallest size
    [SerializeField] private float moveOffsetX = 0.5f;       // Distance to move right during expansion
    [SerializeField] private float moveOffsetY = -0.5f;      // Distance to move down during expansion
    [SerializeField] private float targetOpacity = 0.6f;     // Target opacity (alpha) during expansion

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
            .setEase(LeanTweenType.easeInOutQuad);

        LeanTween.moveLocal(gameObject, expandedPosition, animationTime)
            .setEase(LeanTweenType.easeInOutQuad);

        // Gradually lower the opacity during expansion
        LeanTween.value(gameObject, UpdateOpacity, spriteRenderer.color.a, targetOpacity, animationTime)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                // Hold at the largest size for a few seconds
                Invoke(nameof(StartCollapse), holdAtMaxTime);
            });
    }

    private void StartCollapse()
    {
        // Collapse the object and move it back with ease-in-out
        LeanTween.scale(gameObject, originalScale, animationTime)
            .setEase(LeanTweenType.easeInOutQuad);

        LeanTween.moveLocal(gameObject, originalPosition, animationTime)
            .setEase(LeanTweenType.easeInOutQuad);

        // Gradually restore the opacity during collapse
        LeanTween.value(gameObject, UpdateOpacity, spriteRenderer.color.a, 1.0f, animationTime)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                // Hold at the smallest size for a few seconds
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