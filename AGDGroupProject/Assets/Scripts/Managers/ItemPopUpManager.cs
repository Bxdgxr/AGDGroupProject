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

    // Move previous popups up
    for (int i = 0; i < popUpParent.childCount - 1; i++)
    {
        RectTransform rt = popUpParent.GetChild(i).GetComponent<RectTransform>();
        rt.anchoredPosition += new Vector2(0, rt.rect.height + verticalSpacing);
    }

    // Start fade after delay
    ui.StartFade(displayDuration, 1f); // fades out over 1 second
}


    private IEnumerator DestroyAfterDelay(GameObject popUp)
    {
        yield return new WaitForSeconds(displayDuration);
        Destroy(popUp);
    }
}
