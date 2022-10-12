using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Unit : NetworkBehaviour
{
    public event Action<float> OnHealthChanged;

    public bool IsDestroyed => _health.Value <= 0;
    public bool IsSlowedDown => _slowdownCoroutine != null;
    public string Name => _name;
    public int Cost => _cost;
    public int EXP => _exp;
    public float Health => _health.Value;
    public float MaxHealth => _maxHealth;
    public Team Team => _team;

    [SerializeField] private string _name;
    [SerializeField] private int _cost;
    [SerializeField] private int _exp;
    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _damage;
    [SerializeField] private float _damageSpeed;

    protected NavMeshAgent _navMeshAgent;

    private PlayerBalance _playerBalance;
    private Gates _gates;
    private Team _team;
    private Transform _gatesTransform;
    private IEnumerator _attackCoroutine;
    private IEnumerator _slowdownCoroutine;

    private NetworkVariable<float> _health = new NetworkVariable<float>();


    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _navMeshAgent.speed = _baseSpeed;

        _health.Value = _maxHealth;

        if(IsHost == false)
            _health.OnValueChanged += (hp, newHp) => OnHealthChanged?.Invoke(newHp);
    }

    public void Init(Gates gates, Team team, PlayerBalance playerBalance)
    {
        _gates = gates;
        _team = team;
        _playerBalance = playerBalance;

        _navMeshAgent.SetDestination(gates.TargetPoint.position);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(float damage, int count = 0)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        if (count == 0)
            TakeDamage(damage);
        else
            StartCoroutine(DamageCoroutine(damage, count));
    }

    [ServerRpc(RequireOwnership = false)]
    public void SlowdownServerRpc(float multiplier, int duration)
    {
        if (multiplier > 1 || duration < 0)
            throw new ArgumentOutOfRangeException();

        _slowdownCoroutine = Slowdown(multiplier, duration);
        StartCoroutine(_slowdownCoroutine);

        SlowdownClientRpc(multiplier, duration);
    }

    [ClientRpc]
    private void SlowdownClientRpc(float multiplier, int duration)
    {
        _slowdownCoroutine = Slowdown(multiplier, duration);
        StartCoroutine(_slowdownCoroutine);
    }

    private IEnumerator Slowdown(float multiplier, int duration)
    {
        _navMeshAgent.speed = Mathf.Min(_baseSpeed * multiplier, _navMeshAgent.speed);

        yield return new WaitForSeconds(duration);

        _navMeshAgent.speed = _baseSpeed;
        _slowdownCoroutine = null;
    }

    private IEnumerator DamageCoroutine(float damage, int count)
    {
        if (count <= 0)
            yield break;

        yield return new WaitForSeconds(1f);

        TakeDamage(damage);

        StartCoroutine(DamageCoroutine(damage, count - 1));
    }

    private void TakeDamage(float damage)
    {
        _health.Value -= damage;
        OnHealthChanged?.Invoke(_health.Value);

        if (_health.Value <= 0)
        {
            if (_team == Team.First)
                _playerBalance.IncreaseEXP(_exp);
            else
                GiveEXPClientRpc();
            GetComponent<NetworkObject>().Despawn(true);
        }
    }

    [ClientRpc]
    private void GiveEXPClientRpc()
    {
        if (IsHost)
            return;

        _playerBalance.IncreaseEXP(_exp);
    }

    private void FixedUpdate()
    {
        if (IsOwner && IsNextToGates())
        {
            if (_attackCoroutine == null && _gates != null)
            {
                _attackCoroutine = AttackGates();
                StartCoroutine(_attackCoroutine);
            }
        }
    }

    private IEnumerator AttackGates()
    {
        yield return new WaitForSeconds(_damageSpeed);

        _gates.TakeDamageServerRpc(_damage);

        _attackCoroutine = AttackGates();
        StartCoroutine(_attackCoroutine);
    }

    private bool IsNextToGates()
    {
        Vector3 targetPosition = _gates == null ? _gatesTransform.position : _gates.TargetPoint.position;
        Vector2 targetPoint = new Vector2(targetPosition.x, targetPosition.z);
        Vector2 destination = new Vector2(_navMeshAgent.destination.x, _navMeshAgent.destination.z);

        return !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance 
            && targetPoint == destination && (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f);
    }
}
