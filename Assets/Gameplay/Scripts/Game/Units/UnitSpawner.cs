using UnityEngine;
using Unity.Netcode;
using Zenject;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class UnitSpawner : NetworkBehaviour
{
    public bool CanSendUnits => _delayCoroutine == null;
    public float SpawnDelay => _spawnDelay;
    public int MaxUnitsCount => _maxUnitsCount;

    [SerializeField] private Transform _firstSpawner;
    [SerializeField] private Transform _secondSpawner;
    [SerializeField] private Gates _firstGates;
    [SerializeField] private Gates _secondGates;
    [SerializeField] private int _maxUnitsCount;
    [SerializeField] private float _spawnDelay;

    [Inject] private PlayerBalance _playerBalance;
    [Inject] private Units _unitsData;
    [Inject] private Team _team;

    private IEnumerator _delayCoroutine;

    public void Send(Dictionary<int, int> units)
    {
        if (units.Values.Sum() > _maxUnitsCount)
            throw new System.InvalidOperationException();

        foreach (var unit in units)
        {
            for(int i = 0; i < unit.Value; i++)
                SendUnitServerRpc(_team, NetworkManager.LocalClientId, unit.Key);
        }

        _delayCoroutine = Delay();
        StartCoroutine(_delayCoroutine);
    }

    public void SetMaxUnits(int count)
    {
        if (count <= 0)
            throw new System.ArgumentOutOfRangeException(nameof(count));

        _maxUnitsCount = count;
    }
    public void SetSpawnDelay(float time)
    {
        if (time < 0)
            throw new System.ArgumentOutOfRangeException(nameof(time));

        _spawnDelay = time;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendUnitServerRpc(Team fromTeam, ulong owner, int unitIndex)
    {
        Unit unit = Instantiate(_unitsData[unitIndex].Unit, fromTeam == Team.First ? _secondSpawner : _firstSpawner);
        unit.Init(GetGates(fromTeam), fromTeam, _playerBalance);

        NetworkObject networkUnit = unit.GetComponent<NetworkObject>();
        networkUnit.SpawnWithOwnership(owner);

        SendUnitClientRpc(fromTeam, networkUnit.NetworkObjectId);
    }

    [ClientRpc]
    private void SendUnitClientRpc(Team fromTeam, ulong unitId)
    {
        if (NetworkManager.IsHost)
            return;

        Unit unit = NetworkManager.SpawnManager.SpawnedObjects[unitId].GetComponent<Unit>();
        unit.Init(GetGates(fromTeam), fromTeam, _playerBalance);
    }

    private Gates GetGates(Team team)
    {
        return team == Team.First ? _secondGates : _firstGates;
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(_spawnDelay);

        _delayCoroutine = null;
    }
}
