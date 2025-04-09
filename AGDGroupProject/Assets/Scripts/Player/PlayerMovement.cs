using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movementInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get movement input
        movementInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                movementInput.y += 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                movementInput.y -= 1;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                movementInput.x -= 1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                movementInput.x += 1;
        }

        // Normalize diagonal movement
        movementInput = movementInput.normalized;
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.linearVelocity = movementInput * moveSpeed;
    }
}
