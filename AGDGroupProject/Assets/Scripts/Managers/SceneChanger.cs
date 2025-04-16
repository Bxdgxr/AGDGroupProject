using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && SaveData.canTeleport)
        {
            other.GetComponent<PlayerPositionHandler>().SavePosition();
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
