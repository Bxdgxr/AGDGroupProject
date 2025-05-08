using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public delegate void OnHealthChanged(int current, int max);
    public event OnHealthChanged onHealthChanged;

    [Header("Knockback & Invincibility")]
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float invincibilityDuration = 1f;
    private bool isKnockedBack = false;
    private bool isInvincible = false;
    private float invincibilityTimer;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    [Header("Death Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth, maxHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
        }
    }

    private void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
                SetOpacity(1f);
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
            StartCoroutine(ApplyKnockback(knockbackDirection, knockbackDistance));
            isInvincible = true;
            invincibilityTimer = invincibilityDuration;
            SetOpacity(0.5f);
        }
    }

    public void Heal(int amount)
    {
        if (currentHealth <= 0) return;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private IEnumerator ApplyKnockback(Vector2 knockbackDirection, float knockbackDistance)
    {
        if (rb == null) yield break;

        isKnockedBack = true;
        float time = 0f;
        Vector2 start = rb.position;
        Vector2 end = start + knockbackDirection.normalized * knockbackDistance;

        while (time < knockbackDuration)
        {
            rb.MovePosition(Vector2.Lerp(start, end, time / knockbackDuration));
            time += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(end);
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
        StartCoroutine(FadeToBlackAndTransition());
    }

    private IEnumerator FadeToBlackAndTransition()
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Overworld1_1");
    }

    public bool IsInvincible => isInvincible;
}
