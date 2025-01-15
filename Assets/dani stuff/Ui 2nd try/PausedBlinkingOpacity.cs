using UnityEngine;

public class PausedBlinkingOpacity : MonoBehaviour
{
    [Header("Settings")]
    public float pauseDuration = 4.0f;     // Time to pause before and after the animation
    public float cycleDuration = 4.0f;    // Total time for the animation cycle
    public int repeatCount = 4;           // Number of opacity changes in the animation
    public float minOpacity = 0.2f;       // Minimum opacity (20%)
    public float maxOpacity = 1.0f;       // Maximum opacity (100%)

    private SpriteRenderer spriteRenderer; // SpriteRenderer for controlling opacity

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start the initial delay and begin the cycle loop
        Invoke(nameof(StartOpacityCycle), pauseDuration);
    }

    private void StartOpacityCycle()
    {
        float timePerChange = cycleDuration / repeatCount; // Time for each transition (fade in or fade out)

        for (int i = 0; i < repeatCount; i++)
        {
            // Alternate between max and min opacity
            float startOpacity = (i % 2 == 0) ? minOpacity : maxOpacity;
            float targetOpacity = (i % 2 == 0) ? maxOpacity : minOpacity;

            // Schedule the opacity change
            LeanTween.value(gameObject, UpdateOpacity, startOpacity, targetOpacity, timePerChange)
                .setEase(LeanTweenType.easeInOutSine)  // Smooth transition for both directions
                .setDelay(i * timePerChange);          // Stagger each transition
        }

        // Schedule the next pause-animation loop
        LeanTween.delayedCall(gameObject, cycleDuration, StartPause);
    }

    private void StartPause()
    {
        // Pause before restarting the animation
        LeanTween.delayedCall(gameObject, pauseDuration, StartOpacityCycle);
    }

    private void UpdateOpacity(float newAlpha)
    {
        // Update the sprite's alpha value
        Color color = spriteRenderer.color;
        color.a = newAlpha;
        spriteRenderer.color = color;
    }
}
