using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BalanceUpgradeVisualElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _costValue;
    [SerializeField] private TextMeshProUGUI _efficiencyValue;
    [SerializeField] private GameObject _lockedPanel;
    [SerializeField] private Button _upgradeButton;

    private IReadOnlyList<PlayerBalanceUpgrade> _playerBalanceUpgrades;
    private int _currentUpgrade;
    private bool _isLocked;
    private UpgradesMenu _upgradesMenu;
    private PlayerBalance _playerBalance;

    public void Init(UpgradesMenu upgradesMenu, PlayerBalance playerBalance)
    {
        _playerBalanceUpgrades = upgradesMenu.Upgrades;
        _upgradesMenu = upgradesMenu;
        _playerBalance = playerBalance;

        _upgradeButton.onClick.AddListener(TryUpgrade);

        UpdateUI(_playerBalanceUpgrades[0]);
    }

    private void TryUpgrade()
    {
        if (_upgradesMenu.TryUpgrade(_playerBalanceUpgrades[_currentUpgrade]) == false || _isLocked)
            return;

        if (_currentUpgrade == _playerBalanceUpgrades.Count - 1)
        {
            Lock();
            return;
        }

        _currentUpgrade += 1;
        UpdateUI(_playerBalanceUpgrades[_currentUpgrade]);
    }

    private void UpdateUI(PlayerBalanceUpgrade playerBalanceUpgrade)
    {
        _costValue.text = playerBalanceUpgrade.EXPCost.ToString();
        _efficiencyValue.text = (playerBalanceUpgrade.Multiplier * _playerBalance.BaseEfficiency).ToString();
    }

    private void Lock()
    {
        _isLocked = true;
        _lockedPanel.SetActive(true);
    }
}
