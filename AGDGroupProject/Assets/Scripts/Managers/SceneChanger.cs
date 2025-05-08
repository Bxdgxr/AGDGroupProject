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
            var inventory = FindAnyObjectByType<InventoryManager>();
            var hotbar = FindAnyObjectByType<HotbarManager>(); // or whatever script holds your hotbar list

            if (inventory != null)
                InventoryData.Instance?.SaveInventory(inventory.slots);

            if (hotbar != null)
                InventoryData.Instance?.SaveHotbar(hotbar.hotbarSlots);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
