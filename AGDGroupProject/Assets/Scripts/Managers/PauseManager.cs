using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void LoadMainMenu()
    {
    {
        // Clear player name
        if (PlayerProfile.Instance != null)
            PlayerProfile.Instance.ResetName();

        // Clear inventory and hotbar
        if (InventoryData.Instance != null)
            InventoryData.Instance.ClearAll();
    }
        Time.timeScale = 1f; // Ensure game unfreezes
        SceneManager.LoadScene("Main Menu"); // Replace with your menu scene name
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
}
