using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Collider2D doorCollider;

    void Start()
    {
        doorCollider = GetComponent<Collider2D>();
        if (doorCollider == null)
        {
            Debug.LogWarning("DoorController: No Collider found on this object.");
        }
    }

    public void OpenDoor()
    {
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
            Debug.Log("Door opened: Collider disabled.");
        }

        // Optional: Add visual/sound feedback here
    }
}
