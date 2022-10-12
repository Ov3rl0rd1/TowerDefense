using Zenject;
using UnityEngine;

public class PlayerUIMonoInstaller : MonoInstaller
{
    [SerializeField] private TowersBuyMenu _towersBuyMenu;
    [SerializeField] private MenuManager _menuManager;

    public override void InstallBindings()
    {
        Container.Bind<TowersBuyMenu>().FromInstance(_towersBuyMenu).AsSingle();
        Container.Bind<MenuManager>().FromInstance(_menuManager).AsSingle();
    }
}
