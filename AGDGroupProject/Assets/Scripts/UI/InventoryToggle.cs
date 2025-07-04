using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryUI;
    public KeyCode toggleKey = KeyCode.I;  // Change to Tab or custom key

    void Update()
    {
        if (Input.GetKeyDown(toggleKey)&& !DialogueRumpleEnd.IsInputActive)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }
}
