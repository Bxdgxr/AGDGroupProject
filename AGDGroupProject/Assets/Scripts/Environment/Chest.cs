using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    public Sprite closedSprite;
    public Sprite openedSprite;

    private SpriteRenderer spriteRenderer;
    private bool isOpened = false;
    private bool isPlayerNearby = false;

    private GiveMultipleItems itemGiver;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedSprite;
        itemGiver = GetComponent<GiveMultipleItems>();
    }

    private void Update()
    {
        if (isPlayerNearby && !isOpened && Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryOpenChest();
        }
    }

    public void TryOpenChest()
    {
        if (isOpened) return;

        isOpened = true;
        spriteRenderer.sprite = openedSprite;

        if (itemGiver != null)
        {
            itemGiver.GiveAllToPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
