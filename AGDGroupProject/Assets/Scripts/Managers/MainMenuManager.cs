using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject nameInputPanel;
    public GameObject creditsPanel;

    [Header("Reference to Name Entry UI Script")]
    public NameEntryUI nameEntryUI;

    [Header("Next Scene Name")]
    public string gameSceneName = "GameScene"; // Set this in the Inspector

    // Called when Start Game is pressed
    public void OnStartButtonPressed()
    {
        mainMenuPanel.SetActive(false);
        nameInputPanel.SetActive(true);
    }

    // Called when Submit is pressed
    public void OnNameSubmitted()
    {
        nameEntryUI.SetPlayerName();

        // Optionally hide the input panel
        nameInputPanel.SetActive(false);

        // Load the next scene
        SceneManager.LoadScene(gameSceneName);
    }
    public void OnCreditsButtonPressed()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }
    public void OnBackButtonPressed()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
