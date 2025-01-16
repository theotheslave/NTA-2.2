using UnityEngine;

public class CircleExpandVisualAid : MonoBehaviour
{
    [Header("Settings")]
    public float expandSize = 1.5f;
    public float animationTime = 0.5f;
    public float delayBetween = 0.2f;
    public float holdAtMinTime = 2.0f;
    public float moveOffsetX = 0.5f;
    public float moveOffsetY = -0.5f;
    public float targetOpacity = 0.6f;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private SpriteRenderer spriteRenderer;
    private float originalOpacity; // Store the original opacity

    void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on this GameObject!");
            enabled = false;
            return;
        }

        originalOpacity = spriteRenderer.color.a; // Store the original opacity
        StartExpandCollapse();
    }

    private void StartExpandCollapse()
    {
        Vector3 expandedPosition = originalPosition + new Vector3(moveOffsetX, moveOffsetY, 0);

        // Start ALL animations concurrently
        LeanTween.scale(gameObject, originalScale * expandSize, animationTime)
            .setEase(LeanTweenType.easeInOutQuad);

        LeanTween.moveLocal(gameObject, expandedPosition, animationTime)
            .setEase(LeanTweenType.easeInOutQuad);

        LeanTween.value(gameObject, UpdateOpacity, originalOpacity, targetOpacity, animationTime)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                Invoke(nameof(StartCollapse), holdAtMinTime);
            });
    }

    private void StartCollapse()
    {
        // Start ALL animations concurrently
        LeanTween.scale(gameObject, originalScale, animationTime)
            .setEase(LeanTweenType.easeInOutQuad);

        LeanTween.moveLocal(gameObject, originalPosition, animationTime)
            .setEase(LeanTweenType.easeInOutQuad);

        LeanTween.value(gameObject, UpdateOpacity, targetOpacity, originalOpacity, animationTime) // Back to original opacity
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                Invoke(nameof(StartExpandCollapse), holdAtMinTime);
            });
    }

    private void UpdateOpacity(float newAlpha)
    {
        Color color = spriteRenderer.color;
        color.a = newAlpha;
        spriteRenderer.color = color;
    }
}