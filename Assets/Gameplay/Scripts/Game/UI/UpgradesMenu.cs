using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UpgradesMenu : BaseMenu
{
    public IReadOnlyList<BaseUpgrade> Upgrades => _upgrades;

    [SerializeField] private UpgradesVisitor _upgradesVisitor;
    [SerializeField] private List<BaseUpgrade> _upgrades;
    [SerializeField] private GameObject _upgradesMenu;
    [SerializeField] private Transform _upgradesContent;

    [Inject] private MenuManager _menuManager;

    public override bool IsActive => _upgradesMenu.activeInHierarchy;


    private void Start()
    {
        foreach (var upgrade in _upgrades)
        {
            UpgradeVisualElement upgradeUI = Instantiate(upgrade.VisualElement, _upgradesContent);
            upgradeUI.Init(this, upgrade);
        }
    }

    public void TryUpgrade(BaseUpgrade baseUpgrade)
    {
        baseUpgrade.Accept(_upgradesVisitor);
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
