using UnityEngine;

[CreateAssetMenu(fileName = "CoinGeneratorUpgrade", menuName = "Data/Upgrades/Coin Generator", order = 0)]
public class PlayerBalanceUpgrade : BaseUpgrade
{
    protected override Upgrade[] _upgrades => _balanceUpgrades;

    [SerializeField] private BalanceUpgrade[] _balanceUpgrades;

    public override void Accept(UpgradesVisitor visitor)
    {
        visitor.Visit(this);
    }

    public float GetUpgrade()
    {
        if (CanUpgrade == false)
            throw new System.InvalidOperationException();

        _currentLevel += 1;
        return _balanceUpgrades[_currentLevel].Multiplier;
    }

    [System.Serializable]
    private class BalanceUpgrade : Upgrade
    {
        public float Multiplier;
    }
}
