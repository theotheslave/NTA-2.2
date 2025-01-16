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
    private bool canCalibrate = true;
    public float calibrationDelay = 2f; // Duration for calibration
    [SerializeField] private Slider calibrationSlider; // Reference to the Slider UI
    public MultiSliderAndPanelTransition panelManager;
    public bool IsCalibrated => isCalibrated; 
    public float RelativeYPosition => transform.position.y - originPoint.y;

    private Coroutine calibrationCoroutine;

    void Update()
    {
        if (!panelManager.AllPanelsComplete)
        {
            return; 
        }
        // Start calibration on button press
        if (OVRInput.Get(OVRInput.Button.Two))
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

        
        calibrationSlider.value = 0;

        float elapsedTime = 0f;

        while (elapsedTime < calibrationDelay)
        {
            // If the trigger button is released, stop calibration
            if (!OVRInput.Get(OVRInput.Button.Two))
            {
                ResetCalibrationUI();
                yield break;
            }

           
            elapsedTime += Time.deltaTime;
            calibrationSlider.value = elapsedTime / calibrationDelay; 
            yield return null;
        }

        
        originPoint = transform.position;
        isCalibrated = true;
        isCalibrating = false;


       
        calibrationSlider.value = 1;

        Debug.Log($"Origin calibrated at: {originPoint}");

        
        OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.RTouch); 
    }

    private void ResetCalibrationUI()
    {
        isCalibrating = false;
        calibrationSlider.value = 0; 
        Debug.Log("Calibration canceled.");
    }


    public void DisableCalibration()
    {
        canCalibrate = false;
        if (isCalibrating && calibrationCoroutine != null)
        {
            StopCoroutine(calibrationCoroutine);
            ResetCalibrationUI();
        }
        Debug.Log("Calibration disabled.");
    }
}