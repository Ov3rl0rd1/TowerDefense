using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class TowerVisualElement : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnBuyClicked => _buyButton.onClick;

    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _cost;

    public void Init(BaseTower tower)
    {
        _name.text = tower.Name;
        _cost.text = ((int)tower.Cost).ToString();
    }
}
