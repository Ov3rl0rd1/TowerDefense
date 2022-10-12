using UnityEngine;
using Unity.Netcode;
using Zenject;

public class UnitSpawner : NetworkBehaviour
{
    [SerializeField] private Transform _firstSpawner;
    [SerializeField] private Transform _secondSpawner;
    [SerializeField] private Gates _firstGates;
    [SerializeField] private Gates _secondGates;

    [Inject] private PlayerBalance _playerBalance;
    [Inject] private Units _unitsData;
    [Inject] private Team _team;

    public void Send(int unitIndex)
    {
        SendUnitServerRpc(_team, NetworkManager.LocalClientId, unitIndex);
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
}
