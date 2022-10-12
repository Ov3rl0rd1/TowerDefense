using UnityEngine;

public class PoisonTowerDamage : BaseTowerDamage
{
    protected override BaseTowerStats[] BaseTowerStats => _towerLevels;

    [SerializeField] private EffectTowerStats[] _towerLevels;

    public override void Damage(Unit unit)
    {
        if (_team != PlayerMonoInstaller.PlayerTeam)
            return;

        unit.TakeDamageServerRpc(_towerLevels[CurrentUpgradeLevel].Damage, _towerLevels[CurrentUpgradeLevel].Duration);
    }
}
