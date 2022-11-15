using System.Collections.Generic;
using UnityEngine;

public class UpgradesMenu : BaseMenu
{
    public IReadOnlyList<BaseUpgrade> Upgrades => _upgrades;

    [SerializeField] private UpgradesVisitor _upgradesVisitor;
    [SerializeField] private List<BaseUpgrade> _upgrades;
    [SerializeField] private Transform _upgradesContent;


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
}
