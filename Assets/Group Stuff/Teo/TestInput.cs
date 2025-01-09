using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;
public class TestInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OVRInput.SetControllerVibration(1.0f, 1f, OVRInput.Controller.LTouch);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {

            Debug.Log("HELP");

        }
    }
}
