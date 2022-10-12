using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Collections;

public abstract class BaseTower : NetworkBehaviour
{
    public Team Team => _team;
    public string Name => _name;
    public float Cost => _cost;

    [SerializeField] protected BaseTowerDamage _towerDamage;
    [SerializeField] protected float _radius;
    [SerializeField] protected TowerCanvas _towerCanvas;

    protected List<Unit> _unitsInRadius = new List<Unit>();
    protected PlayerBalance _playerBalance;
    protected Team _team;

    [SerializeField] private float _cost;
    [SerializeField] private string _name;
    
    private IEnumerator _shootCoroutine;

    public void Init(Team team, PlayerBalance playerBalance)
    {
        _team = team;
        _playerBalance = playerBalance;
        _towerDamage.Init(team);

        StartCoroutine(Tick());
    }

    public bool TryUpgrade()
    {
        if (_playerBalance.Coins < _towerDamage.NextUpgrade.Cost)
            return false;

        _playerBalance.ReduceCoins(_towerDamage.NextUpgrade.Cost);
        _towerDamage.Upgrade();
        return true;
    }

    private IEnumerator Tick()
    {
        if (IsOwner == false)
            yield break;

        if (_unitsInRadius.Count > 0)
        {
            _shootCoroutine = ShootCoroutine();
            StartCoroutine(_shootCoroutine);
            yield break;
        }

        yield return new WaitForSeconds(1f);

        GetTarget();

        StartCoroutine(Tick());
    }

    private IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(1 / _towerDamage.ShootsPerSecond);


        Unit target = GetTarget();

        if (target == null)
        {
            _shootCoroutine = null;
            StartCoroutine(Tick());
            yield break;
        }

        Shoot(target);

        _shootCoroutine = ShootCoroutine();
        StartCoroutine(_shootCoroutine);
    }

    protected virtual void Shoot(Unit target)
    {
        _towerDamage.Damage(target);
    }

    protected virtual Unit GetTarget()
    {
        UpdateUnitsInRadius();
        return _unitsInRadius.Count == 0 ? null : _unitsInRadius[0];
    }

    protected void UpdateUnitsInRadius()
    {
        _unitsInRadius.Clear();

        foreach (var e in Physics.OverlapSphere(transform.position, _radius))
            if (e.TryGetComponent(out Unit unit))
                if (unit.Team != _team && unit.IsDestroyed == false)
                    _unitsInRadius.Add(unit);
    }

    public void OnClick()
    {
        if (_towerDamage.NextUpgrade == null || _team != PlayerMonoInstaller.PlayerTeam)
            return;

        _towerCanvas.EnableUpgradePanel(_towerDamage.NextUpgrade);
    }
}
