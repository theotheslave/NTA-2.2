using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class EndScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup; // Canvas Group for fading
    [SerializeField] private TextMeshProUGUI thankYouText; // Thank you message
    [SerializeField] private float fadeDuration = 2f; // Duration of the fade effect
    [SerializeField] private float messageDisplayDuration = 3f; // How long the message stays visible
    [SerializeField] private GameObject UI;
    private Transform vrCamera; // Reference to the VR camera
    private bool isTransitioning = false;

    private void Start()
    {
        // Get the main camera (VR headset camera)
        vrCamera = Camera.main.transform;

        // Ensure the Canvas starts hidden
        canvasGroup.alpha = 0f;
        thankYouText.alpha = 0f; // Start with the text hidden
    }

    public void TriggerEndScreen()
    {

        UI.SetActive(false);
        if (!isTransitioning)
        {
            isTransitioning = true;

            // Position the canvas in front of the VR user
            PositionCanvasInFrontOfUser();

            // Start the fade process
            StartCoroutine(FadeToBlack());
        }
    }

    private void PositionCanvasInFrontOfUser()
    {
        // Align the Canvas to face the VR user
        Transform canvasTransform = canvasGroup.transform;
        canvasTransform.position = vrCamera.position + vrCamera.forward * 2f; // Place 2 units in front
        canvasTransform.LookAt(vrCamera);
        canvasTransform.Rotate(0, 180, 0); // Correct orientation to face the user
    }

    private IEnumerator FadeToBlack()
    {
        // Gradually fade in the Canvas
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = t / fadeDuration;
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // Display the "Thank you for playing!" message
        StartCoroutine(DisplayThankYouMessage());
    }

    private IEnumerator DisplayThankYouMessage()
    {
        // Fade in the thank-you text
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            thankYouText.alpha = t / fadeDuration;
            yield return null;
        }

        thankYouText.alpha = 1f;

        // Wait for the message to display
        yield return new WaitForSeconds(messageDisplayDuration);

        // End the game or return to the main menu
        Application.Quit(); // Ends the game on Quest devices
    }
}
