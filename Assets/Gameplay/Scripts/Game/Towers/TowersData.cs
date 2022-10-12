using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowersData", menuName = "Data/Towers")]
public class TowersData : ScriptableObject
{
    public BaseTower this[int index]
    {
        get
        {
            return _towersPrefab[index];
        }
    }

    public int Count => _towersPrefab.Count;

    [SerializeField] private List<BaseTower> _towersPrefab;
}
