using UnityEngine;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour
{
    [SerializeField] private GameObject objectToDestroy;
    [SerializeField] private string sceneToLoad;

    private bool isHoleActive = false;
    private SpriteRenderer holeRenderer;
    private Collider2D holeCollider;

    private void Start()
    {
        holeRenderer = GetComponent<SpriteRenderer>();
        holeCollider = GetComponent<Collider2D>();

        // Hide the hole at start
        SetHoleVisible(false);

        if (objectToDestroy == null)
        {
            ActivateHole();
        }
    }

    private void Update()
    {
        if (objectToDestroy == null && !isHoleActive)
        {
            ActivateHole();
        }
    }

    private void ActivateHole()
    {
        isHoleActive = true;
        SetHoleVisible(true);
    }

    private void SetHoleVisible(bool visible)
    {
        if (holeRenderer != null)
        {
            Color c = holeRenderer.color;
            c.a = visible ? 1f : 0f;
            holeRenderer.color = c;
        }

        if (holeCollider != null)
            holeCollider.enabled = visible;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isHoleActive || !other.CompareTag("Player") || !SaveData.canTeleport) return;

        other.GetComponent<PlayerPositionHandler>()?.SavePosition();

        var inventory = FindAnyObjectByType<InventoryManager>();
        var hotbar = FindAnyObjectByType<HotbarManager>();

        if (inventory != null)
            InventoryData.Instance?.SaveInventory(inventory.slots);

        if (hotbar != null)
            InventoryData.Instance?.SaveHotbar(hotbar.hotbarSlots);

        SceneManager.LoadScene(sceneToLoad);
    }
}
