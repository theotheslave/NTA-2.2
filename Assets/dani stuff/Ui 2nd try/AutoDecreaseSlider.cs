using UnityEngine;
using UnityEngine.UI;

public class AutoDecreaseSlider : MonoBehaviour
{
    [Header("Slider Settings")]
    public Slider targetSlider;         // Reference to the UI Slider
    public float decreaseDuration = 3f; // Time (in seconds) for the slider to go from 1 to 0
    public float startDelay = 0f;       // Delay before starting the animation

    private bool isDecreasing = false;

    void Start()
    {
        if (startDelay > 0)
        {
            // Start decreasing after the specified delay
            Invoke(nameof(StartDecreasing), startDelay);
        }
        else
        {
            // Start decreasing immediately
            StartDecreasing();
        }
    }

    private void StartDecreasing()
    {
        if (!isDecreasing && targetSlider != null)
        {
            isDecreasing = true;
            StartCoroutine(DecreaseSliderCoroutine());
        }
    }

    private System.Collections.IEnumerator DecreaseSliderCoroutine()
    {
        float elapsedTime = 0f;

        // Store the initial value of the slider
        float initialValue = targetSlider.value;

        while (elapsedTime < decreaseDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the new value for the slider
            float newValue = Mathf.Lerp(initialValue, 0f, elapsedTime / decreaseDuration);

            targetSlider.value = newValue;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the slider value reaches exactly 0
        targetSlider.value = 0f;

        isDecreasing = false;
    }
}
