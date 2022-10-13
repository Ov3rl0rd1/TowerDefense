using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeVisualElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _costValue;
    [SerializeField] private GameObject _lockedPanel;
    [SerializeField] private Button _upgradeButton;

    private BaseUpgrade _upgrade;
    private bool _isLocked;
    private UpgradesMenu _upgradesMenu;
    private string _costDefaultValue;

    public void Init(UpgradesMenu upgradesMenu, BaseUpgrade baseUpgrade)
    {
        _upgradesMenu = upgradesMenu;
        _upgrade = baseUpgrade;

        _upgradeButton.onClick.AddListener(TryUpgrade);

        _costDefaultValue = _costValue.text;

        UpdateUI(_upgrade);
    }

    private void TryUpgrade()
    {
        if (_isLocked)
            return;

        _upgradesMenu.TryUpgrade(_upgrade);

        if (_upgrade.CanUpgrade == false)
        {
            Lock();
            return;
        }

        UpdateUI(_upgrade);
    }

    private void UpdateUI(BaseUpgrade baseUpgrade)
    {
        _costValue.text = _costDefaultValue + baseUpgrade.Cost.ToString();
        //_efficiencyValue.text = (playerBalanceUpgrade.Multiplier * _playerBalance.BaseEfficiency).ToString();
    }

    private void Lock()
    {
        _isLocked = true;
        _lockedPanel.SetActive(true);
    }
}
