using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    public HotbarManager hotbarManager;

    public int goldCount = 0;
    public float attackRange = 1f;
    public float knockbackStrength = 1f;
    public float chestInteractionRange = 2f;

    private PlayerHealth playerHealth;
    private Animator animator;
    private PlayerMovement playerMovement;

    private bool isSwinging = false;

    private void Start()
    {
        if (hotbarManager == null)
        {
            hotbarManager = FindAnyObjectByType<HotbarManager>();
        }

        playerHealth = GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        if (playerHealth == null) Debug.LogError("PlayerHealth component not found!");
        if (animator == null) Debug.LogError("Animator not found!");
        if (playerMovement == null) Debug.LogError("PlayerMovement script not found!");
    }

    private void Update()
    {
        if (isSwinging) return; // Blokkeer andere acties tijdens zwaai

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            InventoryItemData selectedItem = hotbarManager.GetSelectedItem();

            if (selectedItem != null && selectedItem.category == ItemCategory.Sword)
            {
                StartCoroutine(PlaySwingAnimation());
                TryHitEnemy();
            }
            else if (selectedItem != null && selectedItem.category == ItemCategory.Pickaxe)
            {
                TryMineNearbyOres();
            }
            else if (selectedItem != null && selectedItem.category == ItemCategory.Potion)
            {
                UsePotion();
            }
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryOpenChest();
        }
    }

    IEnumerator PlaySwingAnimation()
    {
        isSwinging = true;
        playerMovement.SetCanMove(false); // Beweeg de speler tijdelijk niet
        animator.SetBool("IsSwinging", true);

        // Sla animatie duurt ~0.4 seconden (pas dit aan aan je clip)
        yield return new WaitForSeconds(0.5f);

        animator.SetBool("IsSwinging", false);
        playerMovement.SetCanMove(true);
        isSwinging = false;
    }

    void TryMineNearbyOres()
    {
        GoldOre[] allOres = FindObjectsByType<GoldOre>(FindObjectsSortMode.None);

        foreach (GoldOre ore in allOres)
        {
            if (ore.IsPlayerInRange())
            {
                bool mined = ore.MineOre();
                break;
            }
        }
    }

    void TryHitEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Enemy") && enemy.gameObject != gameObject)
            {
                BaseEnemy baseEnemy = enemy.GetComponent<BaseEnemy>();
                if (baseEnemy != null)
                {
                    Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                    baseEnemy.TakeDamage(10, knockbackDirection, knockbackStrength);
                }
            }
        }
    }

    void TryOpenChest()
    {
        Collider2D[] chests = Physics2D.OverlapCircleAll(transform.position, chestInteractionRange);

        foreach (Collider2D chestCollider in chests)
        {
            if (chestCollider.CompareTag("Chest"))
            {
                Chest chest = chestCollider.GetComponent<Chest>();
                if (chest != null)
                {
                    chest.TryOpenChest();
                    break;
                }
            }
        }
    }

    void UsePotion()
    {
        HotbarSlotUI slot = hotbarManager.GetSelectedSlot();

        if (slot != null && slot.quantity > 0)
        {
            Debug.Log("Used potion!");
            int healAmount = Mathf.RoundToInt(playerHealth.maxHealth * 0.3f);
            playerHealth.Heal(healAmount);

            slot.UpdateQuantity(slot.quantity - 1);

            if (slot.quantity == 0)
                slot.Clear();
        }
    }
}
