using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UpgradesMenu : BaseMenu
{
    public IReadOnlyList<PlayerBalanceUpgrade> Upgrades => _playerBalanceUpgrades;

    [SerializeField] private List<PlayerBalanceUpgrade> _playerBalanceUpgrades;
    [SerializeField] private GameObject _upgradesMenu;
    [SerializeField] private Transform _upgradesContent;
    [SerializeField] private BalanceUpgradeVisualElement _balanceUpgradeVisualElement;

    [Inject] private PlayerBalance _playerBalance;
    [Inject] private MenuManager _menuManager;

    public override bool IsActive => _upgradesMenu.activeInHierarchy;


    private void Start()
    {
        BalanceUpgradeVisualElement upgradeUI = Instantiate(_balanceUpgradeVisualElement, _upgradesContent);
        upgradeUI.Init(this, _playerBalance);
    }

    public bool TryUpgrade(PlayerBalanceUpgrade playerBalanceUpgrade)
    {
        if (_playerBalance.EXP < playerBalanceUpgrade.EXPCost)
            return false;

        _playerBalance.ReduceEXP(playerBalanceUpgrade.EXPCost);
        _playerBalance.MultiplyGeneratorEfficiency(playerBalanceUpgrade.Multiplier);
        return true;
    }

    public override void Disable()
    {
        _upgradesMenu.SetActive(false);
    }

    public override void Enable()
    {
        if (_menuManager.IsInMenu == false)
            _upgradesMenu.SetActive(true);
    }
}
