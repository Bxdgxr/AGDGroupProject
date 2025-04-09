using UnityEngine;

public class GoldOre : MonoBehaviour
{
    [Header("Ore Properties")]
    public int goldValue = 1;
    public int hitPoints = 1;

    // Now private and only changeable in code
    private float interactionRange = 1.5f;
    private float invincibilityTime = 0.5f;

    private bool isInvincible = false;

    private SpriteRenderer spriteRenderer;
    private Transform player;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public bool IsPlayerInRange()
    {
        if (player == null) return false;
        float distance = Vector2.Distance(transform.position, player.position);
        return distance <= interactionRange;
    }

    public bool MineOre()
    {
        if (isInvincible) return false;

        hitPoints--;

        if (hitPoints <= 0)
        {
            Debug.Log("Ore broken!");
            Destroy(gameObject);
            return true; // Mined successfully
        }
        else
        {
            Debug.Log("Ore hit! " + hitPoints + " hits left.");
            StartCoroutine(TriggerInvincibility());
            return false; // Not mined yet
        }
    }

    private System.Collections.IEnumerator TriggerInvincibility()
    {
        isInvincible = true;
        SetOpacity(0.5f);

        yield return new WaitForSeconds(invincibilityTime);

        SetOpacity(1f);
        isInvincible = false;
    }

    private void SetOpacity(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
