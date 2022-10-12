using UnityEngine;

public class IceTowerDamage : BaseTowerDamage
{
    protected override BaseTowerStats[] BaseTowerStats => _towerLevels;

    [SerializeField] private EffectTowerStats[] _towerLevels;

    public override void Damage(Unit unit)
    {
        if (_team != PlayerMonoInstaller.PlayerTeam)
            return;

        unit.SlowdownServerRpc(_towerLevels[CurrentUpgradeLevel].Damage, _towerLevels[CurrentUpgradeLevel].Duration);
    }
}
