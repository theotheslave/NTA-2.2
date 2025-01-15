using UnityEngine;
using UnityEngine.UI;

public class SliderAndPanelControl : MonoBehaviour
{
    [Header("Slider Settings")]
    public Slider targetSlider;          // The slider to decrease
    public float decreaseDuration = 5.0f; // Time it takes for the slider to go from 1 to 0

    [Header("Panel Shrinking Settings")]
    public RectTransform panelTransform; // RectTransform of the panel to shrink
    public float shrinkDuration = 1.0f;  // Duration of the shrinking effect
    public Vector3 shrinkTargetScale = Vector3.zero; // Scale the panel will shrink to (default: 0, 0, 0)

    private bool isShrinking = false;    // Flag to prevent triggering shrink multiple times

    void Start()
    {
        // Ensure references are assigned
        if (targetSlider == null)
        {
            Debug.LogError("Target Slider is not assigned. Please assign it in the Inspector.");
        }
        if (panelTransform == null)
        {
            Debug.LogError("Panel Transform is not assigned. Please assign it in the Inspector.");
        }

        // Start decreasing the slider value
        StartCoroutine(DecreaseSliderValue());
    }

    private System.Collections.IEnumerator DecreaseSliderValue()
    {
        float elapsedTime = 0f;
        float startValue = targetSlider.value;

        while (elapsedTime < decreaseDuration)
        {
            elapsedTime += Time.deltaTime;

            // Decrease the slider's value gradually
            float newValue = Mathf.Lerp(startValue, 0f, elapsedTime / decreaseDuration);
            targetSlider.value = newValue;

            yield return null; // Wait for the next frame
        }

        // Ensure the slider reaches exactly 0
        targetSlider.value = 0f;

        // Trigger the panel shrinking effect
        StartShrink();
    }

    private void StartShrink()
    {
        if (panelTransform != null && !isShrinking)
        {
            isShrinking = true; // Prevent multiple triggers
            StartCoroutine(ShrinkPanelCoroutine());
        }
    }

    private System.Collections.IEnumerator ShrinkPanelCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 initialScale = panelTransform.localScale;

        while (elapsedTime < shrinkDuration)
        {
            elapsedTime += Time.deltaTime;

            // Gradually shrink the panel's scale
            Vector3 newScale = Vector3.Lerp(initialScale, shrinkTargetScale, elapsedTime / shrinkDuration);
            panelTransform.localScale = newScale;

            yield return null; // Wait for the next frame
        }

        // Ensure the scale reaches exactly zero
        panelTransform.localScale = shrinkTargetScale;

        // Optionally deactivate the panel after shrinking
        panelTransform.gameObject.SetActive(false);
    }
}
