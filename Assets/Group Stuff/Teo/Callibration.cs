using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Oculus.Interaction;
using Oculus.Interaction.Input;
public class Calibration : MonoBehaviour
{
    private Vector3 originPoint;
    private bool isCalibrated = false;
    private bool isCalibrating = false;

    public float calibrationDelay = 2f; // Duration for calibration
    [SerializeField] private Slider calibrationSlider; // Reference to the Slider UI

    public bool IsCalibrated => isCalibrated; // Public property to check calibration status
    public float RelativeYPosition => transform.position.y - originPoint.y;

    private Coroutine calibrationCoroutine;

    void Update()
    {
        // Start calibration on button press
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (!isCalibrating)
            {
                calibrationCoroutine = StartCoroutine(CalibrateOriginWithDelay());
            }
        }
        else
        {
            // Stop calibration if the button is released
            if (isCalibrating && calibrationCoroutine != null)
            {
                StopCoroutine(calibrationCoroutine);
                ResetCalibrationUI();
            }
        }
    }

    private IEnumerator CalibrateOriginWithDelay()
    {
        isCalibrating = true;

        // Reset the slider value
        calibrationSlider.value = 0;

        float elapsedTime = 0f;

        while (elapsedTime < calibrationDelay)
        {
            // If the trigger button is released, stop calibration
            if (!OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                ResetCalibrationUI();
                yield break;
            }

            // Update the slider value
            elapsedTime += Time.deltaTime;
            calibrationSlider.value = elapsedTime / calibrationDelay; // Normalized value (0 to 1)
            yield return null;
        }

        // Set origin point
        originPoint = transform.position;
        isCalibrated = true;
        isCalibrating = false;

        // Ensure the slider is filled
        calibrationSlider.value = 1;

        Debug.Log($"Origin calibrated at: {originPoint}");

        // Optional: Add feedback after calibration is complete
        OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch); // Stop vibration
    }

    private void ResetCalibrationUI()
    {
        isCalibrating = false;
        calibrationSlider.value = 0; // Reset the slider
        Debug.Log("Calibration canceled.");
    }
}