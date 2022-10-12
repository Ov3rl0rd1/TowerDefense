using Unity.Netcode;
using UnityEngine;
using Zenject;

public class PlayerMonoInstaller : MonoInstaller
{
    public static Team PlayerTeam => NetworkManager.Singleton.IsHost ? Team.First : Team.Second;

    [SerializeField] private GameStateSwitcher _gameStateSwitcher;
    [SerializeField] private PauseManager _pauseManager;
    [SerializeField] private PlayerBalance _playerBalance;
    [SerializeField] private TowersSpawner _towersSpawner;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private Gates _firstPlayerGates;
    [SerializeField] private Gates _secondPlayerGates;
    [SerializeField] private TowersData _towersData;
    [SerializeField] private Units _unitsData;

    private bool _isHost => NetworkManager.Singleton.IsHost;
    private Gates _gates => _isHost ? _firstPlayerGates : _secondPlayerGates;

    public override void InstallBindings()
    {
        Container.Bind<Gates>().FromInstance(_gates).AsSingle();
        Container.Bind<UnitSpawner>().FromInstance(_unitSpawner).AsSingle();
        Container.Bind<TowersData>().FromInstance(_towersData).AsSingle();
        Container.Bind<TowersSpawner>().FromInstance(_towersSpawner).AsSingle();
        Container.Bind<PauseManager>().FromInstance(_pauseManager).AsSingle();
        Container.Bind<PlayerBalance>().FromInstance(_playerBalance).AsSingle();
        Container.Bind<Units>().FromInstance(_unitsData).AsSingle();
        Container.Bind<GameStateSwitcher>().FromInstance(_gameStateSwitcher).AsSingle();
        Container.Bind<Team>().FromInstance(PlayerTeam).AsSingle();
    }
}
