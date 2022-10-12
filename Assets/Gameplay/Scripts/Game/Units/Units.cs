using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitsData", menuName = "Data/Units")]
public class Units : ScriptableObject
{
    public Data this[int index]
    {
        get
        {
            return _unitsPrefab[index];
        }
    }

    public int IndexOf(Data unit) => _unitsPrefab.IndexOf(unit);
    public int Count => _unitsPrefab.Count;

    [SerializeField] private List<Data> _unitsPrefab;

    [System.Serializable]
    public class Data
    {
        public Unit Unit;
        public bool IsLocked;
        [ConditionalHide(nameof(IsLocked))] public int EXPCost;
    }
}
