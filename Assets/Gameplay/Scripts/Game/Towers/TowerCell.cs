using Unity.Netcode;
using UnityEngine;
using Zenject;

public enum Team 
{ 
    None,
    First, 
    Second 
}

public class TowerCell : MonoBehaviour
{

    public BaseTower Tower => _tower;

    [SerializeField] private Transform _towerHolder;
    [SerializeField] private Team _cellTeam;


    [Inject] private TowersSpawner _towersSpawner;
    [Inject] private PauseManager _pauseManager;
    [Inject] private Team _team;

    private BaseTower _tower;

    private bool _canInteract => _cellTeam == _team && _tower == null;

    public void Attach(BaseTower tower)
    {
        if (_tower != null)
            return;

        _tower = tower;
        _tower.transform.position = _towerHolder.position;
    }

    public void OnClick()
    {
        if (_canInteract == false || _pauseManager.IsPaused)
            return;

        _towersSpawner.SelectCell(this);
    }
}
