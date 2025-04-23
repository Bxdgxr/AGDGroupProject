using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class BaseEnemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public GiveItem giveItem; // Reference to the GiveItem script

    [Header("Health Settings")]
    [SerializeField] protected int maxHealth = 50;
    protected int currentHealth;
    protected bool isInIframe = false;
    protected float iframeTimer = 0f;
    protected float iframeDuration = 1f;

    [Header("Damage Settings")]
    [SerializeField] protected int damageAmount = 10;
    [SerializeField] protected float damageCooldown = 1f;
    protected float damageTimer;

    [Header("Knockback Settings")]
    [SerializeField] protected float knockbackStrength = 5f;
    [SerializeField] protected float knockbackDuration = 0.5f;

    [Header("UI")]
    protected Slider healthBarSlider;
    protected GameObject healthBarObject;

    protected Color originalColor;

    protected virtual void Start()
    {
        currentHealth = maxHealth;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;

        // Health bar setup
        healthBarSlider = GetComponentInChildren<Slider>();
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
            healthBarObject = healthBarSlider.gameObject;
            healthBarObject.SetActive(false);
        }

        // Rigidbody2D moet Dynamic zijn, zodat de vijand kan botsen met de muren.
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    protected virtual void Update()
    {
        if (isInIframe)
        {
            iframeTimer -= Time.deltaTime;
            if (iframeTimer <= 0f)
            {
                isInIframe = false;
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
            }
        }

        damageTimer -= Time.deltaTime;
    }

    // Player collision check (trigger voor de speler)
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (damageTimer > 0f) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Vector2 knockbackDirection = other.transform.position - transform.position;
                playerHealth.TakeDamage(damageAmount, knockbackDirection, knockbackStrength);
                damageTimer = damageCooldown;
            }
        }
    }

    // When the enemy gets damaged
    public virtual void TakeDamage(int damageAmount, Vector2 knockbackDirection, float knockbackForce)
    {
        if (isInIframe) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
            if (currentHealth < maxHealth)
                healthBarObject.SetActive(true);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            isInIframe = true;
            iframeTimer = iframeDuration;

            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
            StartCoroutine(StopKnockback());
        }
    }

    protected virtual IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);
        rb.linearVelocity = Vector2.zero;
    }

    protected virtual void Die()
    {
        if (giveItem != null)
        {
            giveItem.GiveToPlayer(); // Call the method to give gold
        }
        else
        {
            Debug.LogWarning("GiveItem script not assigned to BaseEnemy!");
        }
        Destroy(gameObject);
    }
}
