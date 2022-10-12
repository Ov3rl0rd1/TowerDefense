using System.Linq;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public bool IsInMenu => _menus.Any(x => x.IsActive);

    [SerializeField] private BaseMenu[] _menus;
}
