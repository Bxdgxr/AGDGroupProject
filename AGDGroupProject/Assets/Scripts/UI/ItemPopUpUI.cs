using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ItemPopUpUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI quantityText;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Setup(Sprite icon, string itemName, int quantity)
    {
        iconImage.sprite = icon;
        nameText.text = itemName;
        quantityText.text = quantity > 1 ? "x" + quantity.ToString() : "";
    }

    public void StartFade(float delay, float fadeDuration)
    {
        StartCoroutine(FadeOut(delay, fadeDuration));
    }

    private IEnumerator FadeOut(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        float startAlpha = canvasGroup.alpha;
        float timer = 0f;

        while (timer < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        Destroy(gameObject);
    }
}
