using NaughtyAttributes;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class PlayerMonoInstaller : MonoInstaller
{
    public static Team PlayerTeam => NetworkManager.Singleton.IsHost ? Team.First : Team.Second;

    [SerializeField, Required] private GameStateSwitcher _gameStateSwitcher;
    [SerializeField, Required] private PauseManager _pauseManager;
    [SerializeField, Required] private PlayerBalance _playerBalance;
    [SerializeField, Required] private TowersSpawner _towersSpawner;
    [SerializeField, Required] private UnitSpawner _unitSpawner;
    [SerializeField, Required] private Gates _firstPlayerGates;
    [SerializeField, Required] private Gates _secondPlayerGates;
    [SerializeField, Required] private TowersData _towersData;
    [SerializeField, Required] private Units _unitsData;

    private Gates _gates => PlayerTeam == Team.Second ? _secondPlayerGates : _firstPlayerGates;

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
