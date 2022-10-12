using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.Events;

public class Gates : NetworkBehaviour
{
    public UnityEvent OnDestroy;
    public UnityEvent<float> OnHealthChanged;
    public float Health => _health.Value;
    public float MaxHealth => _maxHealth;
    public Transform TargetPoint => _targetPoint;

    [SerializeField] private float _maxHealth;
    [SerializeField] private Transform _targetPoint;

    private NetworkVariable<float> _health = new NetworkVariable<float>();

    private void Awake()
    {
        _health.Value = _maxHealth;
            
        _health.OnValueChanged += (p, n) => OnHealthChanged?.Invoke(n);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(float damage)
    {
        if (_health.Value == 0)
            return;

        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        _health.Value = Mathf.Max(0, _health.Value - damage);

        if (_health.Value == 0)
            OnDestroy?.Invoke();
    }
}
