using UnityEngine;

public abstract class BaseUpgrade : ScriptableObject
{
    public UpgradeVisualElement VisualElement => _upgradeUI;
    public int Cost => CanUpgrade ? _upgrades[_currentLevel + 1].EXPCost : 0;
    public bool CanUpgrade => _currentLevel + 1 < _upgrades.Length;

    protected abstract Upgrade[] _upgrades { get; }

    protected int _currentLevel = -1;

    [SerializeField] private UpgradeVisualElement _upgradeUI;

    public abstract void Accept(UpgradesVisitor visitor);

    [System.Serializable]
    protected class Upgrade
    {
        public int EXPCost;
    }
}
