using UnityEngine;

public class TowerDamage : BaseTowerDamage
{
    protected override BaseTowerStats[] BaseTowerStats => _towerStats;

    [SerializeField] private BaseTowerStats[] _towerStats;
}
