using UnityEngine;

[CreateAssetMenu(fileName = "UnitsSendDelayUpgrade", menuName = "Data/Upgrades/Units Send Delay", order = 0)]
public class UnitsSendDelayUpgrade : BaseUpgrade
{
    protected override Upgrade[] _upgrades => _sendDelayUpgrades;

    [SerializeField] private SendDelayUpgrade[] _sendDelayUpgrades;

    public override void Accept(UpgradesVisitor visitor)
    {
        visitor.Visit(this);
    }

    public float GetUpgrade()
    {
        if (CanUpgrade == false)
            throw new System.InvalidOperationException();

        _currentLevel += 1;
        return _sendDelayUpgrades[_currentLevel].Delay;
    }

    [System.Serializable]
    private class SendDelayUpgrade : Upgrade
    {
        public float Delay;
    }
}
