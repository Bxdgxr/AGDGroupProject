using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Animator animator;
    private bool canMove = true;

    // Direction booleans
    public bool isLookingUp = false;
    public bool isLookingDown = false;
    public bool isLookingLeft = false;
    public bool isLookingRight = false;

    // Movement
    private bool isMoving = false;

    // Track key presses
    private bool upHeld, downHeld, leftHeld, rightHeld;
    private string currentDirection = "down"; // default facing

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canMove)
        {
            movementInput = Vector2.zero;
            animator.SetBool("IsMoving", false);
            return;
        }

        movementInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            upHeld = Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed;
            downHeld = Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed;
            leftHeld = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed;
            rightHeld = Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed;

            if (leftHeld) movementInput.x -= 1;
            if (rightHeld) movementInput.x += 1;
            if (upHeld) movementInput.y += 1;
            if (downHeld) movementInput.y -= 1;

            HandleDirectionSwitch();
        }

        movementInput = movementInput.normalized;
        isMoving = movementInput != Vector2.zero;

        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsLookingUp", isLookingUp);
        animator.SetBool("IsLookingDown", isLookingDown);
        animator.SetBool("IsLookingLeft", isLookingLeft);
        animator.SetBool("IsLookingRight", isLookingRight);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    private void HandleDirectionSwitch()
    {
        switch (currentDirection)
        {
            case "up":
                if (!upHeld) CheckNewDirectionPriority(); break;
            case "down":
                if (!downHeld) CheckNewDirectionPriority(); break;
            case "left":
                if (!leftHeld) CheckNewDirectionPriority(); break;
            case "right":
                if (!rightHeld) CheckNewDirectionPriority(); break;
        }
    }

    private void CheckNewDirectionPriority()
    {
        if (upHeld) SetDirection("up");
        else if (downHeld) SetDirection("down");
        else if (leftHeld) SetDirection("left");
        else if (rightHeld) SetDirection("right");
    }

    private void SetDirection(string direction)
    {
        isLookingUp = isLookingDown = isLookingLeft = isLookingRight = false;

        switch (direction)
        {
            case "up": isLookingUp = true; break;
            case "down": isLookingDown = true; break;
            case "left": isLookingLeft = true; break;
            case "right": isLookingRight = true; break;
        }

        currentDirection = direction;
    }

    public void TriggerSwingAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Swing");
            animator.SetBool("IsSwinging", true);
            StartCoroutine(ResetSwingFlag());
        }
    }

    private System.Collections.IEnumerator ResetSwingFlag()
    {
        yield return new WaitForSeconds(0.4f); // lengte van swing-animatie
        animator.SetBool("IsSwinging", false);
    }
}
