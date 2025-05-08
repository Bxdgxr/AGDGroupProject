using UnityEngine;

public class VisualEffectsData : MonoBehaviour
{
    public static VisualEffectsData Instance;

    public bool greyscaleActive = false;
    public bool vignetteActive = false;
    public bool distortionActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveVisualEffects(bool grey, bool vignette, bool distortion)
    {
        greyscaleActive = grey;
        vignetteActive = vignette;
        distortionActive = distortion;
    }

    public void LoadVisualEffects(out bool grey, out bool vignette, out bool distortion)
    {
        grey = greyscaleActive;
        vignette = vignetteActive;
        distortion = distortionActive;
    }
}
