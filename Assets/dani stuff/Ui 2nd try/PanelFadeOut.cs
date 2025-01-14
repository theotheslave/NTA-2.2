using UnityEngine;
using UnityEngine.UI;

public class PanelFadeOut : MonoBehaviour
{
    [Header("Fade Settings")]
    public Slider targetSlider;          // The slider to monitor
    public CanvasGroup panelCanvasGroup; // CanvasGroup for the panel to fade out
    public float fadeDuration = 1.0f;    // Duration of the fade-out effect

    private bool isFading = false;

    void Update()
    {
        // Check if the slider's value is 0 and the fade-out process hasn't started yet
        if (targetSlider.value <= 0 && !isFading)
        {
            StartFadeOut();
        }
    }

    private void StartFadeOut()
    {
        if (panelCanvasGroup != null)
        {
            isFading = true; // Prevent multiple triggers
            StartCoroutine(FadeOutCoroutine());
        }
    }

    private System.Collections.IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        float initialAlpha = panelCanvasGroup.alpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the new alpha value
            float newAlpha = Mathf.Lerp(initialAlpha, 0f, elapsedTime / fadeDuration);
            panelCanvasGroup.alpha = newAlpha;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the alpha reaches exactly 0 and disable the panel
        panelCanvasGroup.alpha = 0f;
        panelCanvasGroup.gameObject.SetActive(false); // Deactivate the panel if needed
        isFading = false;
    }
}
