using Unity.Netcode;
using UnityEngine;
using Zenject;

public class TowerDamageField : BaseTowerDamage
{
    protected override BaseTowerStats[] BaseTowerStats => _towerLevels;

    [SerializeField] private DamageField _damageField;
    [SerializeField] private EffectTowerStats[] _towerLevels;

    public override void Damage(Unit unit)
    {
        if (_team != PlayerMonoInstaller.PlayerTeam)
            return;

        SpawnDamageFieldServerRpc(NetworkManager.Singleton.LocalClientId, unit.transform.position, 
            _towerLevels[CurrentUpgradeLevel].Damage, _towerLevels[CurrentUpgradeLevel].Duration, _team);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnDamageFieldServerRpc(ulong owner, Vector3 position, float damage, int duration, Team team)
    {
        DamageField spawnedPrefab = Instantiate(_damageField, position, Quaternion.identity);
        NetworkObject networkObject = spawnedPrefab.GetComponent<NetworkObject>();
        networkObject.SpawnWithOwnership(owner);

        InitOnClientRpc(networkObject.NetworkObjectId, damage, duration, team);

        spawnedPrefab.Init(damage, duration, team);
    }

    [ClientRpc]
    private void InitOnClientRpc(ulong id, float damage, int duration, Team team)
    {
        NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].GetComponent<DamageField>().Init(damage, duration, team);
    }
}

[System.Serializable]
public class EffectTowerStats : BaseTowerStats
{
    public int Duration => _duration;

    [SerializeField] private int _duration;
}
