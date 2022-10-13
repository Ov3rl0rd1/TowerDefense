using UnityEngine;

[CreateAssetMenu(fileName = "MaxUnitsCountUpgrade", menuName = "Data/Upgrades/Max Units", order = 0)]
public class MaxUnitsCountUpgrade : BaseUpgrade
{
    protected override Upgrade[] _upgrades => _maxCountUpgrades;

    [SerializeField] private MaxCountUpgrade[] _maxCountUpgrades;

    public override void Accept(UpgradesVisitor visitor)
    {
        visitor.Visit(this);
    }

    public int GetUpgrade()
    {
        if (CanUpgrade == false)
            throw new System.InvalidOperationException();

        _currentLevel += 1;
        return _maxCountUpgrades[_currentLevel].MaxCount;
    }

    [System.Serializable]
    private class MaxCountUpgrade : Upgrade
    {
        public int MaxCount;
    }
}
