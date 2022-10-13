using UnityEngine;
using Zenject;

public class UpgradesVisitor : MonoBehaviour
{
    [Inject] private PlayerBalance _playerBalance;
    [Inject] private UnitSpawner _unitSpawner;

    public void Visit(PlayerBalanceUpgrade playerBalanceUpgrade)
    {
        if (TryVisitBaseUpgrade(playerBalanceUpgrade))
            _playerBalance.MultiplyGeneratorEfficiency(playerBalanceUpgrade.GetUpgrade());
    }

    public void Visit(UnitsSendDelayUpgrade unitsSendDelayUpgrade)
    {
        if (TryVisitBaseUpgrade(unitsSendDelayUpgrade))
            _unitSpawner.SetSpawnDelay(unitsSendDelayUpgrade.GetUpgrade());
    }

    public void Visit(MaxUnitsCountUpgrade maxUnitsCountUpgrade)
    {
        if (TryVisitBaseUpgrade(maxUnitsCountUpgrade))
            _unitSpawner.SetMaxUnits(maxUnitsCountUpgrade.GetUpgrade());
    }

    private bool TryVisitBaseUpgrade(BaseUpgrade baseUpgrade)
    {
        if (baseUpgrade.CanUpgrade && _playerBalance.Coins >= baseUpgrade.Cost)
        {
            _playerBalance.ReduceEXP(baseUpgrade.Cost);
            return true;
        }

        return false;
    }
}
