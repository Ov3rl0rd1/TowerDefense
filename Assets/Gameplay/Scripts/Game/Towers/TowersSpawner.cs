using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Zenject;

public class TowersSpawner : NetworkBehaviour
{
    [SerializeField] private List<TowerCell> _towerCells;

    [Inject] private PlayerBalance _playerBalance;
    [Inject] private TowersData _towers;
    [Inject] private MenuManager _menuManager;
    [Inject] private TowersBuyMenu _buyMenu;
    [Inject] private Team _playerTeam;

    private int? _selectedCellIndex;

    public void SelectCell(TowerCell cell)
    {
        if (_menuManager.IsInMenu)
            return;

        _selectedCellIndex = _towerCells.IndexOf(cell);
        _buyMenu.Enable();
        _buyMenu.OnBuyTower.AddListener(PlaceTower);
    }

    private void PlaceTower(int towerIndex)
    {
        if (_selectedCellIndex.HasValue == false)
            return;

        PlaceTowerServerRpc(towerIndex, _selectedCellIndex.Value, NetworkManager.LocalClientId, _playerTeam);
        _selectedCellIndex = null;
    }

    [ServerRpc(RequireOwnership = false)]
    private void PlaceTowerServerRpc(int towerIndex, int selectedCell, ulong clientId, Team team)
    {
        BaseTower tower = Instantiate(_towers[towerIndex]);

        NetworkObject networkTower = tower.GetComponent<NetworkObject>();
        networkTower.SpawnWithOwnership(clientId);

        tower.Init(team, _playerBalance);
        _towerCells[selectedCell].Attach(tower);

        PlaceTowerClientRpc(networkTower.NetworkObjectId, selectedCell, team);
    }

    [ClientRpc]
    private void PlaceTowerClientRpc(ulong networkObjectId, int cell, Team team)
    {
        if (IsHost)
            return;

        NetworkObject networkTower = NetworkManager.SpawnManager.SpawnedObjects[networkObjectId];
        BaseTower tower = networkTower.GetComponent<BaseTower>();
        _towerCells[cell].Attach(tower);
        tower.Init(team, _playerBalance);
    }
}
