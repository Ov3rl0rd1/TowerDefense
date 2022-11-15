using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Zenject;
using System.Collections;
using UnityEngine.UI;

public class UnitsSendMenu : BaseMenu
{
    public int MaxUnitsCount => _unitsSpawner.MaxUnitsCount;
    public int SelectedUnitsCount => _selectedUnitsId.Select(x => x.Value).Sum();

    [SerializeField] private Button _sendButton;
    [SerializeField] private TextMeshProUGUI _sendButtonText;
    [SerializeField] private Transform _unitsContent;
    [SerializeField] UnitVisualElement _unitPrefab;
    [SerializeField] TextMeshProUGUI _totalCostText;

    [Inject] private GameStateSwitcher _gameStateSwitcher;
    [Inject] private UnitSpawner _unitsSpawner;
    [Inject] private Units _unitsData;
    [Inject] private PlayerBalance _playerBalance;

    private Dictionary<Units.Data, int> _selectedUnitsId = new Dictionary<Units.Data, int>();
    private string _defaultSendText;
    private int _totalCost;

    private void Start()
    {
        _defaultSendText = _sendButtonText.text;
        for (int i = 0; i < _unitsData.Count; i++)
        {
            UnitVisualElement unitUI = Instantiate(_unitPrefab, _unitsContent);
            unitUI.Init(_unitsData[i], this);
            _selectedUnitsId.Add(_unitsData[i], 0);
        }
    }

    public void TrySend()
    {
        if (_gameStateSwitcher.CurrentState.CanSendUnits == false)
            return;

        int totalCost = 0;

        foreach (var e in _selectedUnitsId)
            if (e.Value > 0)
                totalCost += e.Key.Unit.Cost * e.Value;

        if (_playerBalance.Coins < totalCost)
            return;

        _playerBalance.ReduceCoins(totalCost);

        Dictionary<int, int> units = new Dictionary<int, int>();

        foreach (var e in _selectedUnitsId)
            units.Add(_unitsData.IndexOf(e.Key), e.Value);
        _unitsSpawner.Send(units);

        StartCoroutine(Delay(_unitsSpawner.SpawnDelay));
    }

    public bool TryUnlock(Units.Data unit)
    {
        if (_playerBalance.EXP < unit.EXPCost)
            return false;

        _playerBalance.ReduceEXP(unit.EXPCost);
        return true;
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

    private IEnumerator Delay(float time)
    {
        _sendButton.interactable = false;

        for (int i = 0; i < time; i++)
        {
            _sendButtonText.text = _defaultSendText + $"({(int)time - i})";
            yield return new WaitForSeconds(1);
        }

        _sendButtonText.text = _defaultSendText;
        _sendButton.interactable = true;
    }
}
