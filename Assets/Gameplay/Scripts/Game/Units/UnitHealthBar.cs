using UnityEngine;
using TMPro;

public class UnitHealthBar : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private GameObject _healthPanel;
    [SerializeField] private TextMeshProUGUI _healthValue;

    private Camera _playerCamera;

    private void Awake()
    {
        _playerCamera = Camera.main;
    }

    private void OnEnable()
    {
        _healthPanel.SetActive(true);
        _unit.OnHealthChanged += HealthChanged;
    }

    private void OnDisable()
    {
        _healthPanel.SetActive(false);
        _unit.OnHealthChanged -= HealthChanged;
    }

    private void Update()
    {
        transform.LookAt(_playerCamera.transform, Vector3.up);
        transform.eulerAngles += new Vector3(90, 180, 0);
    }

    private void HealthChanged(float health)
    {
        if (health <= 0)
            gameObject.SetActive(false);

        string healthBar = "";

        for (int i = 0; i < (int)(health / _unit.MaxHealth * 10); i++)
            healthBar += "|";

        _healthValue.text = healthBar;
    }
}
