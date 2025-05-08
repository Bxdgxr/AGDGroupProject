using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CharacterMover : MonoBehaviour
{
    public Transform characterToMove;
    public Transform targetPoint;
    public float moveSpeed = 2f;
    public PlayerMovement playerMovement;
    public DialogueManager dialogueManager;

    public UnityEvent onArrive;

    public void MoveCharacter()
    {
        if (characterToMove == null || targetPoint == null) return;

        StartCoroutine(MoveRoutine());
    }

    public void CatSit()
    {
        Animator animator = characterToMove.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Sit");
            animator.SetBool("IsLookingRight", false);
            animator.SetBool("IsMoving", false);
        }
    }

    public void CatStand()
    {
        Animator animator = characterToMove.GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Stand");
    }

    private IEnumerator MoveRoutine()
    {
        Animator animator = characterToMove.GetComponent<Animator>();
        Vector3 direction = (targetPoint.position - characterToMove.position).normalized;

        SetFacingDirection(animator, direction);
        if (animator != null)
            animator.SetBool("IsMoving", true);

        while (Vector3.Distance(characterToMove.position, targetPoint.position) > 0.05f)
        {
            characterToMove.position = Vector3.MoveTowards(characterToMove.position, targetPoint.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        characterToMove.position = targetPoint.position;

        if (animator != null)
            animator.SetBool("IsMoving", false);

        // ✅ Disable player movement now that they arrived
        if (playerMovement != null)
            playerMovement.enabled = false;

        onArrive?.Invoke();

        if (dialogueManager != null)
            dialogueManager.NotifyLineFinished();
    }

    private void SetFacingDirection(Animator animator, Vector3 direction)
    {
        if (animator == null) return;

        animator.SetBool("IsLookingUp", false);
        animator.SetBool("IsLookingDown", false);
        animator.SetBool("IsLookingLeft", false);
        animator.SetBool("IsLookingRight", false);

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            animator.SetBool("IsLookingRight", direction.x > 0);
            animator.SetBool("IsLookingLeft", direction.x < 0);
        }
        else
        {
            animator.SetBool("IsLookingUp", direction.y > 0);
            animator.SetBool("IsLookingDown", direction.y < 0);
        }
    }
}
