using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitVisualElement : MonoBehaviour
{
    [SerializeField] private GameObject _lockedPanel;
    [SerializeField] private Button _addButton;
    [SerializeField] private Button _removeButton;
    [SerializeField] private Button _unlockButton;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _count;
    [SerializeField] private TextMeshProUGUI _expValue;

    private int _selectedCount;
    private UnitsSendMenu _unitsSendMenu;
    private Units.Data _unit;
    private bool _isLocked;

    public void Init(Units.Data unit, UnitsSendMenu unitsSendMenu)
    {
        _name.text = unit.Unit.Name;
        _count.text = "0";

        _unitsSendMenu = unitsSendMenu;
        _unit = unit;

        _lockedPanel.SetActive(unit.IsLocked);
        _expValue.text = unit.EXPCost.ToString();
        _isLocked = unit.IsLocked;

        _addButton.onClick.AddListener(TryAdd);
        _removeButton.onClick.AddListener(TryRemove);
        _unlockButton.onClick.AddListener(TryUnlock);
    }

    private void TryUnlock()
    {
        if (_isLocked == false)
            return;

        if (_unitsSendMenu.TryUnlock(_unit))
        {
            _lockedPanel.SetActive(false);
            _isLocked = false;
        }
    }

    private void TryAdd()
    {
        if (_unitsSendMenu.SelectedUnitsCount >= _unitsSendMenu.MaxUnitsCount || _isLocked)
            return;

        _unitsSendMenu.Add(_unit);
        _selectedCount += 1;
        _count.text = _selectedCount.ToString();
    }

    private void TryRemove()
    {
        if (_selectedCount <= 0 || _isLocked)
            return;

        _unitsSendMenu.Remove(_unit);
        _selectedCount -= 1;
        _count.text = _selectedCount.ToString();
    }
}
