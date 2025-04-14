using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Health Reference")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Animation Settings")]
    [SerializeField] private float fillSpeed = 5f;

    private float targetValue;

    private void Start()
    {
        if (playerHealth != null)
        {
            playerHealth.onHealthChanged += UpdateHealthBar;
            UpdateHealthBar(playerHealth.currentHealth, playerHealth.maxHealth);
        }
    }

    private void Update()
    {
        if (healthSlider != null && !Mathf.Approximately(healthSlider.value, targetValue))
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetValue, Time.deltaTime * fillSpeed);
        }
    }

    private void UpdateHealthBar(int current, int max)
    {
        targetValue = current;

        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
        }

        if (healthText != null)
        {
            healthText.text = $"{current} / {max}";
        }
    }
}
