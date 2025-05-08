using System.Collections.Generic;
using UnityEngine;

public class TapestryPuzzleManager : MonoBehaviour
{
    [Tooltip("Define the correct order by matching the plate IDs")]
    public List<string> correctOrder = new List<string>();
    public DoorController door; // Drag the door object here in Unity

    private List<TapestryPlate> currentSequence = new List<TapestryPlate>();
    private bool puzzleSolved = false;

    public void PlateStepped(TapestryPlate plate)
    {
        if (puzzleSolved) return;

        currentSequence.Add(plate);

        if (!IsCorrectSoFar())
        {
            ResetPuzzle();
            return;
        }

        if (currentSequence.Count == correctOrder.Count)
        {
            Debug.Log("Puzzle completed!");
            PuzzleCompleted();
        }
    }

    private bool IsCorrectSoFar()
    {
        for (int i = 0; i < currentSequence.Count; i++)
        {
            if (currentSequence[i].plateID != correctOrder[i])
                return false;
        }
        return true;
    }

    private void ResetPuzzle()
    {
        foreach (var plate in currentSequence)
            plate.ResetPlate();

        currentSequence.Clear();

        // Optional: play reset sound/effects here
        Debug.Log("Wrong order. Puzzle reset.");
    }

private void PuzzleCompleted()
{
    puzzleSolved = true;
    Debug.Log("Puzzle solved!");
    if (door != null)
    {
        door.OpenDoor();
    }
}

}
