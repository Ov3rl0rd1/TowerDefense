using UnityEngine;
using Zenject;

public abstract class BaseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menuObject;

    [Inject] private MenuManager _menuManager;

    public bool IsActive => _menuObject.activeSelf;

    public void Enable()
    {
        if (_menuManager.IsInMenu == false)
            _menuObject.SetActive(true);
    }

    public void Disable()
    {
        _menuObject.SetActive(false);
    }
}
