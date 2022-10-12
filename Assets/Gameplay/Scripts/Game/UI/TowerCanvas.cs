using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class TowerCanvas : MonoBehaviour
{
    [SerializeField] private BaseTower _tower;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _towerLevel;
    [SerializeField] private TextMeshProUGUI _upgradeCost;
    [SerializeField] private TextMeshProUGUI _fromLevel;
    [SerializeField] private TextMeshProUGUI _toLevel;
    [SerializeField] private GameObject _towerLevelPanel;
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private Button _upgradeButton;

    private Camera _playerCamera;
    private BaseTowerStats _upgrade;
    private int _currentLevel = 1;

    private void Start()
    {
        _playerCamera = Camera.main;
        _canvas.worldCamera = _playerCamera;

        if (_tower.IsOwner == false)
            return;

        _towerLevelPanel.SetActive(true);
        _towerLevel.text = _currentLevel.ToRoman();
    }

    public void EnableUpgradePanel(BaseTowerStats upgrade)
    {
        if (_tower.IsOwner == false || _upgradePanel.activeInHierarchy)
            return;

        _upgradeButton.onClick.AddListener(TryUpgrade);

        _upgrade = upgrade;
        _upgradeCost.text = upgrade.Cost.ToString();
        _upgradePanel.SetActive(true);
        _towerLevelPanel.SetActive(false);

        _fromLevel.text = _currentLevel.ToRoman();
        _toLevel.text = (_currentLevel+1).ToRoman();
    }
    
    private void TryUpgrade()
    {
        if (_upgrade == null || _tower.TryUpgrade() == false || _tower.IsOwner == false)
            return;

        _currentLevel += 1;
        _towerLevelPanel.gameObject.SetActive(true);
        _upgradePanel.SetActive(false);
        _towerLevel.text = _currentLevel.ToRoman();
        _upgradeButton.onClick.RemoveListener(TryUpgrade);
    }

    private void Update()
    {
        if (_tower.IsOwner == false)
            return;

        _canvas.transform.LookAt(_playerCamera.transform, Vector3.up);
        _canvas.transform.eulerAngles += new Vector3(90, 180, 0);
    }
}
