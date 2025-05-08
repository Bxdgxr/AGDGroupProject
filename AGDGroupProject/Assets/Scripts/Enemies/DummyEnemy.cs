using UnityEngine;
using System.Collections;

public class DummyEnemy : BaseEnemy
{
    [Header("Regeneration")]
    [SerializeField] private float regenAmount = 10f;
    [SerializeField] private float regenInterval = 1f;
    public Animator animator;

    protected override void Start()
    {
        maxHealth = 100;
        base.Start();
        StartCoroutine(RegenerateHealth());
    }

    protected override void Update()
    {
        base.Update();
        rb.linearVelocity = Vector2.zero; // Geen beweging
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        // Dummy doet geen schade
    }

    public override void TakeDamage(int damageAmount, Vector2 knockbackDirection, float knockbackForce)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        animator.SetTrigger("Hit");
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
            healthBarObject.SetActive(true);
        }

        // Dummy blijft altijd leven
        if (currentHealth <= 0)
        {
            currentHealth = 1;
        }
        // Geen knockback of iFrames
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        StartCoroutine(ResetColor());
    }

    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenInterval);

            if (currentHealth < maxHealth)
            {
                currentHealth += (int)regenAmount;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

                if (healthBarSlider != null)
                    healthBarSlider.value = currentHealth;
            }
        }
    }

    private IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
    }

    protected override void Die()
    {
        // Dummy gaat niet dood
    }
}
