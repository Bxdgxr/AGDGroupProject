using UnityEngine;
using System.Collections;

public class GoblinEnemy : BaseEnemy
{
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private float moveSpeed = 2.5f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolRange = 3f;
    [SerializeField] private float patrolSpeed = 1.5f;
    [SerializeField] private float patrolPauseTime = 2f;

    private bool isKnockedBack = false;
    private Vector2 patrolStartPoint;
    private bool isPatrolling = true;
    private int patrolDirection = 1; // 1 = rechts, -1 = links
    private float patrolPauseTimer = 0f;

    protected override void Start()
    {
        base.Start();
        patrolStartPoint = transform.position;
    }

    protected override void Update()
    {
        base.Update();

        if (isKnockedBack || player == null || animator.GetBool("IsDead")) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            isPatrolling = false;

            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;

            SetDirectionAnimation(direction);
        }
        else
        {
            if (!isPatrolling)
            {
                isPatrolling = true;
                patrolPauseTimer = 0f;
            }

            Patrol();
        }
    }

    private void SetDirectionAnimation(Vector2 direction)
    {
        // Reset all direction booleans
        animator.SetBool("faceFront", false);
        animator.SetBool("faceBack", false);
        animator.SetBool("faceLeft", false);
        animator.SetBool("faceRight", false);

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
                animator.SetBool("faceRight", true);
            else
                animator.SetBool("faceLeft", true);
        }
        else
        {
            if (direction.y > 0)
                animator.SetBool("faceBack", true);
            else
                animator.SetBool("faceFront", true); // Front = facing south
        }
    }

    private void ResetDirectionAnimation()
    {
        // Ensure no direction animation is playing if idle
        animator.SetBool("faceFront", false);
        animator.SetBool("faceBack", false);
        animator.SetBool("faceLeft", false);
        animator.SetBool("faceRight", false);
    }

    private void Patrol()
    {
        if (patrolPauseTimer > 0f)
        {
            patrolPauseTimer -= Time.deltaTime;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 target = patrolStartPoint + Vector2.right * patrolRange * patrolDirection;
        Vector2 direction = (target - (Vector2)transform.position).normalized;

        rb.linearVelocity = direction * patrolSpeed;
        SetDirectionAnimation(direction);

        // Als hij bijna bij het doel is, wissel van richting en pauzeer even
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            patrolDirection *= -1;
            patrolPauseTimer = patrolPauseTime;
            ResetDirectionAnimation();
        }
    }

    public override void TakeDamage(int damageAmount, Vector2 knockbackDirection, float knockbackForce)
    {
        if (animator.GetBool("IsDead")) return; // Prevent taking damage if dead

        base.TakeDamage(damageAmount, knockbackDirection, knockbackForce);

        isKnockedBack = true;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        StartCoroutine(ResetKnockback());
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;
    }

    protected override void Die()
    {
        if (animator != null && !animator.GetBool("IsDead"))
        {
            // Set all direction booleans to false to ensure no conflicting animations
            ResetDirectionAnimation();

            // Set IsDead to true to trigger the death animation
            animator.SetBool("IsDead", true);

            StartCoroutine(WaitToDie());
        }
        else
        {
            base.Die(); // In case the animator is missing or there's an issue
        }
    }

    private IEnumerator WaitToDie()
    {
        yield return new WaitForSeconds(0.8f); // Adjust to match the death animation length
        // After the death animation ends, disable further updates and stop all behaviors.
        // You can either destroy the object or disable the collider here.
        base.Die();
    }
}
