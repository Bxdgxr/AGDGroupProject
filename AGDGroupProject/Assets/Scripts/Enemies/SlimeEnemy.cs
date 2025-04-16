using UnityEngine;

public class SlimeEnemy : BaseEnemy
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float hopForce = 3f;
    [SerializeField] private float hopCooldown = 1.5f;
    [SerializeField] private float hopDuration = 0.3f;

    private float hopCooldownTimer;
    private float hopMoveTimer;
    private bool isHopping = false;

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
    }
}
