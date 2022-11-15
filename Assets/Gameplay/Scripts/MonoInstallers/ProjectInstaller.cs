using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private GameNetworking _gameNetworking;

    public override void InstallBindings()
    {
        Container.Bind<GameNetworking>().FromInstance(_gameNetworking).AsSingle();
    }
}