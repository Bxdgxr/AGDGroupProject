using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerPositionHandler : MonoBehaviour
{
    private void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // Herstel spelerpositie als die bestaat
        if (SaveData.scenePositions.TryGetValue(currentScene, out Vector2 savedPosition))
        {
            transform.position = savedPosition;
        }

        // Blokkeer teleportatie even bij binnenkomst
        SaveData.canTeleport = false;
        StartCoroutine(EnableTeleportAfterDelay(1.0f));
    }

    public void SavePosition()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SaveData.scenePositions[currentScene] = transform.position;
    }

    private IEnumerator EnableTeleportAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SaveData.canTeleport = true;
    }
}
