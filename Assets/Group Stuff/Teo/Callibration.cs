using UnityEngine;
using System.Collections;
using Oculus.Interaction;
using Oculus.Interaction.Input;
public class Callibration : MonoBehaviour
{
    private Vector3 originPoint; 
    private bool isCalibrated = false; 
    private bool isCalibrating = false; 

    public float calibrationDelay = 2f; 

    void Update()
    {
        // Check if the Oculus trigger button is pressed and start calibration if not already calibrating
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && !isCalibrating)
        {
            StartCoroutine(CalibrateOriginWithDelay());

          
        }


        if (isCalibrated)
        {
            
            Vector3 relativePosition = GetRelativePosition();

            
            HandleYPosition(relativePosition.y);
        }
    }

    private IEnumerator CalibrateOriginWithDelay()
    {
        isCalibrating = true;

        
        OVRInput.SetControllerVibration(1.0f, 0.5f, OVRInput.Controller.RTouch);
        Debug.Log("Calibration starting...");
        yield return new WaitForSeconds(calibrationDelay);

        // new orig pos 
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
        
        if (relativeY > 0.005f)
        {
            OVRInput.SetControllerVibration(1.0f, 0.5f, OVRInput.Controller.RTouch);

            Debug.Log("Object is above 1 unit relative to origin.");
        }
        else if (relativeY <= 0f)
        {
            OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);

            Debug.Log("Object is below -1 unit relative to origin.");
        }
    }
}