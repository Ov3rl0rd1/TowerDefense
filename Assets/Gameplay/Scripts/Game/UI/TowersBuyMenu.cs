using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class TowersBuyMenu : BaseMenu
{
    public UnityEvent<int> OnBuyTower;

    [SerializeField] private Transform _towersContent;
    [SerializeField] private TowerVisualElement _towerPrefab;

    [Inject] private TowersData _towersData;
    [Inject] private PlayerBalance _playerBalance;

    private void Start()
    {
        for (int i = 0; i < _towersData.Count; i++)
        {
            int index = i;
            TowerVisualElement towerUI = Instantiate(_towerPrefab, _towersContent);
            towerUI.Init(_towersData[i]);
            towerUI.OnBuyClicked.AddListener(() => TryBuyTower(index));
        }
    }

    private void TryBuyTower(int index)
    {
        if (_playerBalance.Coins < _towersData[index].Cost)
            return;

        _playerBalance.ReduceCoins(_towersData[index].Cost);

        OnBuyTower?.Invoke(index);
        OnBuyTower.RemoveAllListeners();
        Disable();
    }
}
