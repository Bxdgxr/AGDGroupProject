using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public delegate void OnHealthChanged(int current, int max);
    public event OnHealthChanged onHealthChanged;

    [Header("Knockback & Invincibility")]
    [SerializeField] private float knockbackDuration = 0.2f; // Duration of knockback movement
    [SerializeField] private float invincibilityDuration = 1f;
    private bool isKnockedBack = false;
    private bool isInvincible = false;
    private float invincibilityTimer;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth, maxHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();  // Get Rigidbody2D component
    }

    private void Update()
    {
        // Reduce invincibility timer
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;

            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
                SetOpacity(1f);  // Reset opacity when invincibility ends
            }
        }
    }

    public void TakeDamage(int amount, Vector2 knockbackDirection, float knockbackDistance)
    {
        if (isInvincible || isKnockedBack) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Apply knockback (using Coroutine to smoothly move player)
            StartCoroutine(ApplyKnockback(knockbackDirection, knockbackDistance));

            // Start invincibility frames
            isInvincible = true;
            invincibilityTimer = invincibilityDuration;

            // Set player opacity to 50% while invincible
            SetOpacity(0.5f);
        }
    }

    private IEnumerator ApplyKnockback(Vector2 knockbackDirection, float knockbackDistance)
    {
        if (rb == null) yield break;

        isKnockedBack = true;
        float time = 0f;
        Vector2 start = rb.position;
        Vector2 end = start + knockbackDirection.normalized * knockbackDistance;

        // Smoothly move the player to the knockback destination
        while (time < knockbackDuration)
        {
            rb.MovePosition(Vector2.Lerp(start, end, time / knockbackDuration));
            time += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(end); // Ensure final position is reached
        isKnockedBack = false;
    }

    private void SetOpacity(float opacity)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = opacity;
            spriteRenderer.color = color;
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // TODO: Add death behavior (e.g., restart, respawn)
    }

    public bool IsInvincible => isInvincible;
}
