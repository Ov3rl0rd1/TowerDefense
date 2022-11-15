using Zenject;
using UnityEngine;
using NaughtyAttributes;

public class PlayerUIMonoInstaller : MonoInstaller
{
    [SerializeField, Required] private TowersBuyMenu _towersBuyMenu;
    [SerializeField, Required] private MenuManager _menuManager;

    public override void InstallBindings()
    {
        Container.Bind<TowersBuyMenu>().FromInstance(_towersBuyMenu).AsSingle();
        Container.Bind<MenuManager>().FromInstance(_menuManager).AsSingle();
    }
}
