using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Rigidbody2D rb;

    [Header("Movement Settings")]
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float hopForce = 3f;
    [SerializeField] private float hopCooldown = 1.5f;
    [SerializeField] private float hopDuration = 0.3f;

    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private float knockbackDistance = 1f;  // Adjust the knockback strength per enemy

    private float damageTimer;

    private float hopCooldownTimer;
    private float hopMoveTimer;
    private bool isHopping = false;

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            hopCooldownTimer -= Time.deltaTime;

            if (!isHopping && hopCooldownTimer <= 0f)
            {
                HopTowardsPlayer();
                hopCooldownTimer = hopCooldown;
                hopMoveTimer = hopDuration;
                isHopping = true;
            }
        }

        if (isHopping)
        {
            hopMoveTimer -= Time.deltaTime;

            if (hopMoveTimer <= 0f)
            {
                rb.linearVelocity = Vector2.zero;
                isHopping = false;
            }
        }

        // Count down damage timer
        damageTimer -= Time.deltaTime;
    }

    private void HopTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * hopForce;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Ensure the slime only damages the player when it's within range
        if (damageTimer > 0f) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Calculate knockback direction (opposite of slime's position)
                Vector2 knockbackDirection = other.transform.position - transform.position;

                // Apply damage and knockback to the player
                playerHealth.TakeDamage(damageAmount, knockbackDirection, knockbackDistance); // Passing knockback distance

                damageTimer = damageCooldown;
            }
        }
    }
}
