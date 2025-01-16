using UnityEngine;
using UnityEngine.UI;

public class MultiSliderAndPanelTransition : MonoBehaviour
{
    [Header("References")]
    public Slider[] sliders;                   // Array of sliders for each panel
    public RectTransform[] panelTransforms;    // Array of panels to shrink/expand
    public GameObject breathUI;
    [Header("Settings")]
    public float[] sliderDecreaseTimes;        // Time for each slider to run out
    public float panelTransitionTime = 1.0f;   // Time for panel shrinking/expanding
    public Vector3 panelMaxScale = new Vector3(0.005f, 0.005f, 0.005f); // Max scale for all panels
    public bool AllPanelsComplete { get; private set; } = false;
    private int currentPanelIndex = 0;

    void Start()
    {
        // Ensure all panels are set to inactive except the first
        for (int i = 0; i < panelTransforms.Length; i++)
        {
            panelTransforms[i].localScale = Vector3.zero;
            panelTransforms[i].gameObject.SetActive(false);
            sliders[i].gameObject.SetActive(false); // Hide all sliders initially
        }

        if (breathUI != null) { 
        
            breathUI.SetActive(false);
        
        }

        // Start the process for the first panel
        StartPanelTransition(0);
    }

    void StartPanelTransition(int panelIndex)
    {
        if (panelIndex >= panelTransforms.Length) {

            AllPanelsComplete = true;


            return;
                
                
           } // Safety check for array bounds

        // Activate and expand the current panel
        panelTransforms[panelIndex].gameObject.SetActive(true);
        sliders[panelIndex].gameObject.SetActive(true);
        sliders[panelIndex].value = 1.0f;

        ExpandPanel(panelTransforms[panelIndex], () =>
        {


            if (panelIndex == 2) { 
            
                breathUI.SetActive(true);
            
            }
            // Decrease the slider over its configured time
            LeanTween.value(gameObject, 1.0f, 0.0f, sliderDecreaseTimes[panelIndex])
                .setOnUpdate((float value) =>
                {
                    sliders[panelIndex].value = value;
                })
                .setOnComplete(() =>
                {
                    ShrinkPanel(panelTransforms[panelIndex], () =>
                    {
                        sliders[panelIndex].gameObject.SetActive(false); // Disable the current slider
                        panelTransforms[panelIndex].gameObject.SetActive(false); // Hide the current panel

                        // Start the next panel transition
                        StartPanelTransition(panelIndex + 1);
                    });
                });
        });
    }

    void ShrinkPanel(RectTransform panelTransform, System.Action onComplete)
    {
        LeanTween.scale(panelTransform.gameObject, Vector3.zero, panelTransitionTime)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }

    void ExpandPanel(RectTransform panelTransform, System.Action onComplete)
    {
        LeanTween.scale(panelTransform.gameObject, panelMaxScale, panelTransitionTime)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }
}
