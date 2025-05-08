using UnityEngine;
using System.Collections;

public class SlimeEnemy : BaseEnemy
{
    [Header("Movement Settings")]
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float hopForce = 3f;
    [SerializeField] private float hopCooldown = 1.5f;
    [SerializeField] private float hopDuration = 0.3f;

    private float hopCooldownTimer;
    private float hopMoveTimer;
    private bool isHopping = false;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    protected override void Update()
    {
        base.Update();

        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            hopCooldownTimer -= Time.deltaTime;

            if (!isHopping && hopCooldownTimer <= 0f)
            {
                Vector2 dir = (player.position - transform.position).normalized;
                rb.linearVelocity = dir * hopForce;
                hopCooldownTimer = hopCooldown;
                hopMoveTimer = hopDuration;
                isHopping = true;

                // Animation: hopping
                animator.SetBool("isJumping", true);
                animator.SetBool("isIdle", false);
            }
        }

        if (isHopping)
        {
            hopMoveTimer -= Time.deltaTime;
            if (hopMoveTimer <= 0f)
            {
                rb.linearVelocity = Vector2.zero;
                isHopping = false;

                // Animation: idle
                animator.SetBool("isJumping", false);
                animator.SetBool("isIdle", true);
            }
        }
    }

    protected override void Die()
    {
        if (animator != null)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isIdle", false);
            animator.SetTrigger("isDead");

            // Start coroutine to wait for the death animation to finish
            StartCoroutine(WaitForDeathAnimation());
        }
        else
        {
            base.Die();
        }
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Wait until the "IsDead" animation has finished
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        // Wait until the animation is complete
        yield return new WaitForSeconds(animationDuration);

        // Now call the base Die() method to handle destruction
        base.Die();
    }
}
