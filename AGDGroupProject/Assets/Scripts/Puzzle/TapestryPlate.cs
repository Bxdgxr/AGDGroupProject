using UnityEngine;

public class TapestryPlate : MonoBehaviour
{
    public string plateID; // Unique identifier for this plate (e.g., "Flame", "Raindrop")
    private TapestryPuzzleManager puzzleManager;
    private bool isActivated = false;

    void Start()
    {
        puzzleManager = FindAnyObjectByType<TapestryPuzzleManager>();
        if (puzzleManager == null)
        {
            Debug.LogError("No TapestryPuzzleManager found in scene!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player stepped on plate: {plateID}");
            StepOn();
        }
    }

    public void StepOn()
    {
        if (!isActivated)
        {
            isActivated = true;
            puzzleManager.PlateStepped(this);
        }
    }

    public void ResetPlate()
    {
        isActivated = false;
        // Optionally: visual/audio feedback
    }
}
