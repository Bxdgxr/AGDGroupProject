using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Animator animator;

    // Direction booleans
    private bool isLookingUp = false;
    private bool isLookingDown = false;
    private bool isLookingLeft = false;
    private bool isLookingRight = false;

    // Movement state booleans
    private bool isMoving = false;

    // To track which direction was pressed first
    private string currentDirection = "down"; // Default to down

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movementInput = Vector2.zero;

        // Get input from keyboard for movement
        if (Keyboard.current != null)
        {
            // Check for horizontal input (Left / Right)
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                movementInput.x = -1;
                SetDirection("left");
            }
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                movementInput.x = 1;
                SetDirection("right");
            }

            // Check for vertical input (Up / Down)
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            {
                movementInput.y = 1;
                SetDirection("up");
            }
            else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            {
                movementInput.y = -1;
                SetDirection("down");
            }
        }

        // Normalize the movement input to prevent diagonal speed increase
        movementInput = movementInput.normalized;

        // Determine if the player is moving or idle
        isMoving = movementInput != Vector2.zero;

        // Set the booleans in the animator for animation transitions
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsLookingUp", isLookingUp);
        animator.SetBool("IsLookingDown", isLookingDown);
        animator.SetBool("IsLookingLeft", isLookingLeft);
        animator.SetBool("IsLookingRight", isLookingRight);
    }

    void FixedUpdate()
    {
        // Apply movement to the Rigidbody2D
        rb.linearVelocity = movementInput * moveSpeed;
    }

    // Method to set direction and reset others
    private void SetDirection(string direction)
    {
        // Only update the direction if it's a new direction
        if (currentDirection != direction)
        {
            // Reset all directions
            isLookingUp = false;
            isLookingDown = false;
            isLookingLeft = false;
            isLookingRight = false;

            // Set the correct direction
            switch (direction)
            {
                case "up":
                    isLookingUp = true;
                    break;
                case "down":
                    isLookingDown = true;
                    break;
                case "left":
                    isLookingLeft = true;
                    break;
                case "right":
                    isLookingRight = true;
                    break;
            }

            // Update the current direction
            currentDirection = direction;
        }
    }
}
