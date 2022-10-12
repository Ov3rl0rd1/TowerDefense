using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUpgrade : ScriptableObject
{
    public int EXPCost => _cost;

    [SerializeField] private int _cost;
}
