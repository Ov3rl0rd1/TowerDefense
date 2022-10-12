using TMPro;
using UnityEngine;
using Zenject;

public class PlayerBalanceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coins;
    [SerializeField] private TextMeshProUGUI _exp;

    [Inject] private PlayerBalance _playerBalance;

    private void Awake()
    {
        _playerBalance.OnBalanceChanged += UpdateBalance;
    }

    private void UpdateBalance()
    {
        _coins.text = ((int)_playerBalance.Coins).ToString();
        _exp.text = ((int)_playerBalance.EXP).ToString();
    }

    private void OnDestroy()
    {
        _playerBalance.OnBalanceChanged -= UpdateBalance;
    }
}
