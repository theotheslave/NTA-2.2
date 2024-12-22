using UnityEngine;
using System.Collections;
using Oculus.Interaction;
using Oculus.Interaction.Input;
public class Callibration : MonoBehaviour
{
    private Vector3 originPoint; // Stores the new origin point
    private bool isCalibrated = false; // Ensures calibration happens only when triggered
    private bool isCalibrating = false; // Prevents multiple calibrations during the delay

    public float calibrationDelay = 2f; // Delay time in seconds

    void Update()
    {
        // Check if the Oculus trigger button is pressed and start calibration if not already calibrating
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && !isCalibrating)
        {
            StartCoroutine(CalibrateOriginWithDelay());
          
        }


        if (isCalibrated)
        {
            // Get the relative position
            Vector3 relativePosition = GetRelativePosition();

            // Example: Perform actions based on Y position
            HandleYPosition(relativePosition.y);
        }
    }

    private IEnumerator CalibrateOriginWithDelay()
    {
        isCalibrating = true;

        // Optional feedback: Notify user calibration is starting
        OVRInput.SetControllerVibration(1.0f, 0.5f, OVRInput.Controller.RTouch);
        Debug.Log("Calibration starting...");
        yield return new WaitForSeconds(calibrationDelay);

        // Set the current position as the new origin
        originPoint = transform.position;
        isCalibrated = true;
        isCalibrating = false;

        Debug.Log($"Origin calibrated at: {originPoint}");
    }

    private Vector3 GetRelativePosition()
    {
        // Calculate the relative position based on the origin
        return transform.position - originPoint;
    }

    private void HandleYPosition(float relativeY)
    {
        
        if (relativeY > 1f)
        {
            Debug.Log("Object is above 1 unit relative to origin.");
        }
        else if (relativeY < -1f)
        {
            Debug.Log("Object is below -1 unit relative to origin.");
        }
    }
}