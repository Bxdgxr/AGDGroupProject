using UnityEngine;
using System.Collections.Generic;

public class GoldOre : MonoBehaviour
{
    [Header("Ore Properties")]
    public int goldValue = 1;
    public int hitPoints = 1;

    public GiveItem giveItem;

    private float interactionRange = 1.5f;
    private float invincibilityTime = 0.5f;

    private bool isInvincible = false;

    private SpriteRenderer spriteRenderer;
    private Transform player;

    // Statische lijst met posities van gemijnde ores tijdens deze speelsessie
    private static HashSet<Vector3> minedPositions = new HashSet<Vector3>();

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Als deze ore al gemijnd is (in deze sessie), verwijder hem
        if (minedPositions.Contains(transform.position))
        {
            Destroy(gameObject);
        }
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
            if (giveItem != null)
            {
                giveItem.GiveToPlayer();
            }
            else
            {
                Debug.LogWarning("GiveItem script not assigned to GoldOre!");
            }

            // Voeg deze positie toe aan de lijst van gemijnde ores
            minedPositions.Add(transform.position);

            Destroy(gameObject);
            return true;
        }
        else
        {
            Debug.Log("Ore hit! " + hitPoints + " hits left.");
            StartCoroutine(TriggerInvincibility());
            return false;
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
