using UnityEngine;

public class SmoothBlendShapeAnimator : MonoBehaviour
{
    public int blendShapeIndex = 0; // Index of the blend shape to animate
    public float animationDuration = 4f; // Duration to animate from 0 to 100
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private bool isAnimating = false;
    private float elapsedTime = 0f;

    private void Start()
    {
        // Get the SkinnedMeshRenderer attached to this object
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("No SkinnedMeshRenderer found on this GameObject!");
        }
        else
        {
            // Optionally start animation on load
            StartBlendShapeAnimation();
        }
    }

    private void Update()
    {
        if (!isAnimating) return;

        // Increment elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate progress as a value between 0 and 1
        float progress = Mathf.Clamp01(elapsedTime / animationDuration);

        // Use SmoothStep for a smoother animation curve
        float smoothProgress = Mathf.SmoothStep(0, 1, progress);
        float weight = Mathf.Lerp(0, 80, smoothProgress);

        // Apply the weight to the blend shape
        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, weight);

        // Stop animating when progress reaches 100%
        if (progress >= 1f)
        {
            isAnimating = false;
            Debug.Log("Smooth blend shape animation finished!");
        }
    }

    public void StartBlendShapeAnimation()
    {
        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer is not set up. Animation cannot start.");
            return;
        }

        // Reset animation state
        elapsedTime = 0f;
        isAnimating = true;
        Debug.Log("Smooth blend shape animation started!");
    }
}
