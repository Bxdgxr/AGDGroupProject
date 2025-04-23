using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public int goldCount = 0;
    public float attackRange = 1f;  // The range for hitting the enemy
    public float knockbackStrength = 1f;  // Knockback strength when hitting the enemy
    public float chestInteractionRange = 2f; // Range for opening chest

    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            TryMineNearbyOres();
            TryHitEnemy();
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryOpenChest();
        }
    }

    void TryMineNearbyOres()
    {
        GoldOre[] allOres = FindObjectsByType<GoldOre>(FindObjectsSortMode.None);

        foreach (GoldOre ore in allOres)
        {
            if (ore.IsPlayerInRange())
            {
                bool mined = ore.MineOre();
                break; // Only mine one ore per press
            }
        }
    }

    void TryHitEnemy()
    {
        // Find all enemies near the player
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Enemy") && enemy.gameObject != gameObject) // Make sure we don't hit ourselves
            {
                BaseEnemy baseEnemy = enemy.GetComponent<BaseEnemy>();
                if (baseEnemy != null)
                {
                    // Calculate knockback direction (opposite of the player’s position)
                    Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;

                    // Apply damage and knockback
                    baseEnemy.TakeDamage(10, knockbackDirection, knockbackStrength);
                }
            }
        }
    }

    void TryOpenChest()
    {
        // Find all chests within the interaction range
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

}
