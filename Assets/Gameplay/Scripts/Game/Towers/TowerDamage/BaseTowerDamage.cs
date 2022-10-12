using UnityEngine;
using Unity.Netcode;

public abstract class BaseTowerDamage : NetworkBehaviour
{
    public float ShootsPerSecond => BaseTowerStats[CurrentUpgradeLevel].ShootsPerSecond;
    public BaseTowerStats NextUpgrade => MaxUpgradeLevel == CurrentUpgradeLevel + 1 ? null : BaseTowerStats[CurrentUpgradeLevel + 1];
    protected abstract BaseTowerStats[] BaseTowerStats { get; }

    public int CurrentUpgradeLevel { get; private set; }
    public int MaxUpgradeLevel => BaseTowerStats.Length;

    protected Team _team { get; private set; }

    public void Init(Team team)
    {
        _team = team;
    }

    public virtual void Damage(Unit unit)
    {
        unit.TakeDamageServerRpc(BaseTowerStats[CurrentUpgradeLevel].Damage);
    }

    public void Upgrade()
    {
        if (CurrentUpgradeLevel == BaseTowerStats.Length - 1)
            throw new System.InvalidOperationException();

        CurrentUpgradeLevel += 1;
    }
}

[System.Serializable]
public class BaseTowerStats
{
    public int Cost => _cost;
    public float Damage => _damage;
    public float ShootsPerSecond => _shootsPerSecond;

    [SerializeField] private int _cost;
    [SerializeField] private float _damage;
    [SerializeField] private float _shootsPerSecond;
}
