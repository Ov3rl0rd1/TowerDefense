using UnityEngine;

public abstract class BaseMenu : MonoBehaviour
{
    public abstract bool IsActive { get; }

    public abstract void Enable();
    public abstract void Disable();
}
