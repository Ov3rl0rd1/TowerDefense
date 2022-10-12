using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Zenject;
using System.Collections;

public class UnitsSendMenu : BaseMenu
{
    public int MaxUnitsCount => _maxUnitsCount;
    public int SelectedUnitsCount => _selectedUnitsId.Select(x => x.Value).Sum();

    public override bool IsActive => _sendMenu.activeSelf;

    [SerializeField] private int _maxUnitsCount;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private GameObject _sendMenu;
    [SerializeField] private Transform _unitsContent;
    [SerializeField] UnitVisualElement _unitPrefab;
    [SerializeField] TextMeshProUGUI _totalCostText;

    [Inject] private GameStateSwitcher _gameStateSwitcher;
    [Inject] private UnitSpawner _unitsSpawner;
    [Inject] private Units _unitsData;
    [Inject] private PlayerBalance _playerBalance;
    [Inject] private MenuManager _menuManager;

    private Dictionary<Units.Data, int> _selectedUnitsId = new Dictionary<Units.Data, int>();
    private IEnumerator _delayCoroutine;
    private int _totalCost;

    private void Start()
    {
        for (int i = 0; i < _unitsData.Count; i++)
        {
            UnitVisualElement unitUI = Instantiate(_unitPrefab, _unitsContent);
            unitUI.Init(_unitsData[i], this);
            _selectedUnitsId.Add(_unitsData[i], 0);
        }
    }

    public void TrySend()
    {
        if (_delayCoroutine != null || _gameStateSwitcher.CurrentState.CanSendUnits == false)
            return;

        int totalCost = 0;

        foreach (var e in _selectedUnitsId)
            if (e.Value > 0)
                totalCost += e.Key.Unit.Cost * e.Value;

        if (_playerBalance.Coins < totalCost)
            return;

        _playerBalance.ReduceCoins(totalCost);

        foreach (var e in _selectedUnitsId)
        {
            int index = _unitsData.IndexOf(e.Key);

            for (int i = 0; i < e.Value; i++)
                _unitsSpawner.Send(index);
        }

        _delayCoroutine = Delay();
        StartCoroutine(_delayCoroutine);
    }

    public bool TryUnlock(Units.Data unit)
    {
        if (_playerBalance.EXP < unit.EXPCost)
            return false;

        _playerBalance.ReduceEXP(unit.EXPCost);
        return true;
    }

    public override void Enable()
    {
        if (_menuManager.IsInMenu == false)
            _sendMenu.SetActive(true);
    }

    public override void Disable()
    {
        _sendMenu.SetActive(false);
    }

    public void Add(Units.Data unit)
    {
        if (SelectedUnitsCount >= MaxUnitsCount)
            return;

        _selectedUnitsId[unit] += 1;
        _totalCost += unit.Unit.Cost;
        _totalCostText.text = _totalCost.ToString();
    }

    public void Remove(Units.Data unit)
    {
        if (_selectedUnitsId[unit] <= 0)
            return;

        _selectedUnitsId[unit] -= 1;
        _totalCost -= unit.Unit.Cost;
        _totalCostText.text = _totalCost.ToString();
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(_spawnDelay);

        _delayCoroutine = null;
    }
}
