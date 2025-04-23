using UnityEngine;

public class SkeletonEnemy : BaseEnemy
{
    [Header("Ranged Attack")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootRange = 8f;
    [SerializeField] private float fireRate = 2f;
    private float fireCooldown;

    [Header("Retreat Behavior")]
    [SerializeField] private float retreatRange = 3f;
    [SerializeField] private float retreatSpeed = 1.5f;

    protected override void Update()
    {
        base.Update();

        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Schieten als speler binnen bereik is
        if (distanceToPlayer <= shootRange)
        {
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                ShootArrow();
                fireCooldown = fireRate;
            }
        }

        // Weglopen als speler te dichtbij komt
        if (distanceToPlayer <= retreatRange)
        {
            Vector2 retreatDir = (transform.position - player.position).normalized;
            rb.linearVelocity = retreatDir * retreatSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void ShootArrow()
    {
        if (arrowPrefab == null || firePoint == null) return;

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Arrow arrowScript = arrow.GetComponent<Arrow>();

        if (arrowScript != null)
        {
            Vector2 shootDir = (player.position - firePoint.position).normalized;
            arrowScript.SetDirection(shootDir);
        }
    }

    // Geen melee damage, dus we overriden de trigger en doen niks
    protected override void OnTriggerStay2D(Collider2D other)
    {
        // Leeggelaten om melee damage uit te schakelen
    }
}