using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GatesHealthBar : MonoBehaviour
{
    [SerializeField] private Gates _gates;
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private TextMeshProUGUI _healthText;

    private void Start()
    {
        _gates.OnHealthChanged.AddListener(UpdateValue);

        UpdateValue(_gates.Health);
    }

    private void UpdateValue(float health)
    {
        _healthBarImage.fillAmount = health / _gates.MaxHealth;
        _healthText.text = ((int)health).ToString();
    }
}
