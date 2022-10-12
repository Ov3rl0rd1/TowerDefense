using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class DamageField : NetworkBehaviour
{
    private enum OverlapType { Box, Sphere }

    [SerializeField] private OverlapType _overlapType;
    [SerializeField, ConditionalHide(nameof(_overlapType), 1)] private float _radius;
    [SerializeField, ConditionalHide(nameof(_overlapType), 0)] private Vector3 _center;
    [SerializeField, ConditionalHide(nameof(_overlapType), 0)] private Vector3 _size;
    [SerializeField] private ParticleSystem _particleSystem;

    private int _duration;
    private float _damagePerSecond;
    private Team _team = Team.None;
    private NetworkObject _networkObject;

    public void Init(float damagePerSecond, int duration, Team team)
    {
        _damagePerSecond = damagePerSecond;
        _duration = duration;
        _team = team;

        _networkObject = GetComponent<NetworkObject>();

        StartParticles();
    }

    private void StartParticles()
    {
        _particleSystem.Play();

        if (IsOwner == false)
            return;

        if (_duration != 0)
        {
            StartCoroutine(Damage(_duration));
        }
        else
        {
            DamageUnitsInRadius();
            DespawnServerRpc(_particleSystem.main.duration + 1f);
        }
    }

    private IEnumerator Damage(int count)
    {
        if (count == 0)
        {
            DespawnServerRpc();
            yield break;
        }

        yield return new WaitForSeconds(1);

        DamageUnitsInRadius();

        StartCoroutine(Damage(count - 1));
    }

    [ServerRpc(RequireOwnership = false)]
    private void DespawnServerRpc(float time = 0)
    {
        StartCoroutine(Despawn(time));
    }

    private IEnumerator Despawn(float time)
    {
        yield return new WaitForSeconds(time);

        if (_networkObject.IsSpawned == false)
            yield break;

        _networkObject.Despawn(true);
    }

    private void DamageUnitsInRadius()
    {
        if (_networkObject.IsSpawned == false)
            return;

        Collider[] colliders = _overlapType switch
        {
            OverlapType.Box => Physics.OverlapBox(transform.position + _center, _size),
            OverlapType.Sphere => Physics.OverlapSphere(transform.position, _radius),
            _ => new Collider[0],
        };

        foreach (var e in colliders)
        {
            if (e.TryGetComponent(out Unit unit) && unit.Team != _team && unit.IsDestroyed == false)
            {
                unit.TakeDamageServerRpc(_damagePerSecond);
            }
        }
    }
}
