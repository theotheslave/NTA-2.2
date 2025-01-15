using UnityEngine;
using UnityEngine.UI;

public class SliderAndPanelTransition : MonoBehaviour
{
    [Header("References")]
    public Slider firstSlider;               // First slider
    public Slider secondSlider;              // Second slider
    public RectTransform firstPanelTransform; // First panel to shrink
    public RectTransform secondPanelTransform; // Second panel to expand

    [Header("Settings")]
    public float firstSliderDecreaseTime = 5.0f;   // Time for the first slider to run out
    public float secondSliderDecreaseTime = 5.0f;  // Time for the second slider to run out
    public float panelTransitionTime = 1.0f;       // Time for panel shrinking/expanding
    public Vector3 secondPanelMaxScale = new Vector3(0.005f, 0.005f, 0.005f); // Max size for the second panel

    private bool isTransitioning = false;

    void Start()
    {
        // Ensure both panels and sliders are correctly initialized
        secondPanelTransform.localScale = Vector3.zero;
        secondSlider.gameObject.SetActive(false); // Ensure the second slider starts inactive
        StartFirstPanelTransition();
    }

    void StartFirstPanelTransition()
    {
        firstSlider.value = 1.0f; // Start the first slider at its maximum value
        LeanTween.value(gameObject, 1.0f, 0.0f, firstSliderDecreaseTime)
            .setOnUpdate((float value) =>
            {
                firstSlider.value = value;
            })
            .setOnComplete(() =>
            {
                if (!isTransitioning)
                {
                    isTransitioning = true;
                    ShrinkPanel(firstPanelTransform, () =>
                    {
                        firstSlider.gameObject.SetActive(false); // Disable first slider
                        ExpandPanel(secondPanelTransform, StartSecondPanelSlider);
                    });
                }
            });
    }

    void StartSecondPanelSlider()
    {
        secondSlider.gameObject.SetActive(true); // Enable the second slider
        secondSlider.value = 1.0f; // Start the second slider at its maximum value
        LeanTween.value(gameObject, 1.0f, 0.0f, secondSliderDecreaseTime)
            .setOnUpdate((float value) =>
            {
                secondSlider.value = value;
            })
            .setOnComplete(() =>
            {
                ShrinkPanel(secondPanelTransform, () =>
                {
                    secondSlider.gameObject.SetActive(false); // Disable second slider when done
                });
            });
    }

    void ShrinkPanel(RectTransform panelTransform, System.Action onComplete)
    {
        LeanTween.scale(panelTransform.gameObject, Vector3.zero, panelTransitionTime)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() =>
            {
                panelTransform.gameObject.SetActive(false); // Disable panel after shrinking
                onComplete?.Invoke();
            });
    }

    void ExpandPanel(RectTransform panelTransform, System.Action onComplete)
    {
        panelTransform.gameObject.SetActive(true);
        LeanTween.scale(panelTransform.gameObject, secondPanelMaxScale, panelTransitionTime)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }
}
