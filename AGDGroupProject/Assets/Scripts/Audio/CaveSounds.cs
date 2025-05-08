using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveSounds : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;

    [Tooltip("Scene names where ambient audio should play")]
    public string[] allowedScenes;

    private static CaveSounds instance;

    private void Awake()
    {
        // Singleton zodat er maar één bestaat
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("CaveSounds: Geen AudioSource gevonden!");
            }
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateAudioForScene(scene.name);
    }

    private void UpdateAudioForScene(string sceneName)
    {
        bool shouldPlay = false;
        foreach (string allowed in allowedScenes)
        {
            if (sceneName == allowed)
            {
                shouldPlay = true;
                break;
            }
        }

        if (shouldPlay)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}
