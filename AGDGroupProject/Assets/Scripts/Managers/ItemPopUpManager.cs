using System.Collections;
using UnityEngine;

public class ItemPopUpManager : MonoBehaviour
{
    public GameObject itemPopUpPrefab; // Assign in Inspector
    public Transform popUpParent; // A parent panel anchored bottom-left
    public float displayDuration = 4f; // Editable in Inspector
    public float verticalSpacing = 5f;

    public void ShowPopUp(Sprite icon, string name, int quantity)
    {
        GameObject popUp = Instantiate(itemPopUpPrefab, popUpParent);
        ItemPopUpUI ui = popUp.GetComponent<ItemPopUpUI>();
        ui.Setup(icon, name, quantity);

        // Move other popups upward
        for (int i = 0; i < popUpParent.childCount - 1; i++)
        {
            RectTransform rt = popUpParent.GetChild(i).GetComponent<RectTransform>();
            rt.anchoredPosition += new Vector2(0, rt.rect.height + verticalSpacing);
        }

        StartCoroutine(DestroyAfterDelay(popUp));
    }

    private IEnumerator DestroyAfterDelay(GameObject popUp)
    {
        yield return new WaitForSeconds(displayDuration);
        Destroy(popUp);
    }
}
