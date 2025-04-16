using UnityEngine;
using System.Collections;
public class GoblinEnemy : BaseEnemy
{
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private float moveSpeed = 2.5f;

    private bool isKnockedBack = false; // Flag to check if the goblin is in knockback

    protected override void Update()
    {
        base.Update();

        if (isKnockedBack) return; // Skip movement if in knockback state

        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            rb.linearVelocity = dir * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public override void TakeDamage(int damageAmount, Vector2 knockbackDirection, float knockbackForce)
    {
        base.TakeDamage(damageAmount, knockbackDirection, knockbackForce);
        isKnockedBack = true; // Mark goblin as being in knockback state
        rb.linearVelocity = Vector2.zero; // Immediately stop movement during knockback
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        // Coroutine to reset movement after knockback duration
        StartCoroutine(ResetKnockback());
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false; // Allow goblin to move again after knockback
    }
}
