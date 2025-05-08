using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VisualEffectsController : MonoBehaviour
{
    public Volume volume;

    private ColorAdjustments colorAdjustments;
    private Vignette vignette;
    private LensDistortion lensDistortion;

    private bool greyscaleActive = false;
    private bool vignetteActive = false;
    private bool distortionActive = false;

    private float fadeSpeed = 1.5f;

    private void Start()
    {
        // Load saved visual effects
        if (VisualEffectsData.Instance != null)
        {
            VisualEffectsData.Instance.LoadVisualEffects(out greyscaleActive, out vignetteActive, out distortionActive);
        }

        if (volume != null && volume.profile != null)
        {
            if (volume.profile.TryGet(out colorAdjustments)) { }
            if (volume.profile.TryGet(out vignette)) { }
            if (volume.profile.TryGet(out lensDistortion)) { }
        }
        else
        {
            Debug.LogError("Volume or Volume Profile is missing!");
        }

        if (colorAdjustments != null) colorAdjustments.saturation.value = greyscaleActive ? -90f : 0f;
        if (vignette != null) vignette.intensity.value = vignetteActive ? 0.75f : 0f;
        if (lensDistortion != null) lensDistortion.intensity.value = distortionActive ? -0.45f : 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            ToggleGreyscale();

        if (Input.GetKeyDown(KeyCode.H))
            ToggleVignette();

        if (Input.GetKeyDown(KeyCode.J))
            ToggleDistortion();

        UpdateEffects();
    }

    void UpdateEffects()
    {
        if (colorAdjustments != null)
        {
            float targetSaturation = greyscaleActive ? -90f : 0f;
            colorAdjustments.saturation.value = Mathf.Lerp(colorAdjustments.saturation.value, targetSaturation, Time.deltaTime * fadeSpeed);
        }

        if (vignette != null)
        {
            float targetVignette = vignetteActive ? 0.75f : 0f;
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetVignette, Time.deltaTime * fadeSpeed);
        }

        if (lensDistortion != null)
        {
            float targetDistortion = distortionActive ? -0.45f : 0f;
            lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, targetDistortion, Time.deltaTime * fadeSpeed);
        }
    }

    public void ToggleGreyscale()
    {
        greyscaleActive = !greyscaleActive;
        SaveVisualEffects();
    }

    public void ToggleVignette()
    {
        vignetteActive = !vignetteActive;
        SaveVisualEffects();
    }

    public void ToggleDistortion()
    {
        distortionActive = !distortionActive;
        SaveVisualEffects();
    }

    void SaveVisualEffects()
    {
        if (VisualEffectsData.Instance != null)
        {
            VisualEffectsData.Instance.SaveVisualEffects(greyscaleActive, vignetteActive, distortionActive);
        }
    }
}
