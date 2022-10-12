using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : NetworkBehaviour
{
    private struct Player : INetworkSerializable, IEquatable<Player>
    {
        public ulong Id;
        public bool IsReady;

        public Player(ulong id, bool isReady = false)
        {
            Id = id;
            IsReady = isReady;
        }
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Id);
            serializer.SerializeValue(ref IsReady);
        }

        public bool Equals(Player other) => Id == other.Id && IsReady == other.IsReady;
    }

    [HideInInspector] public UnityEvent<bool> OnStateChanged;

    public bool IsPaused => _isPaused;

    private NetworkList<Player> _readyPlayers = new NetworkList<Player>();
    private bool _isPaused;

    private void Awake()
    {
        if (IsHost == false)
            return;

        foreach (var id in NetworkManager.ConnectedClientsIds)
            _readyPlayers.Add(new Player(id));
    }

    [ServerRpc(RequireOwnership = false)]
    public void PauseGameServerRpc()
    {
        if (_isPaused)
            return;

        Pause();
        PauseGameClientRpc();
    }

    public void SetReadyToContinue(bool value)
    {
        if (_isPaused == false)
            return;

        SetReadyServerRpc(NetworkManager.LocalClientId, value);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetReadyServerRpc(ulong id, bool value)
    {
        bool isAllPlayersReady = true;

        for(int i = 0; i < _readyPlayers.Count; i++)
        {
            if (_readyPlayers[i].Id == id)
                _readyPlayers[i] = new Player(id, value);
            if (_readyPlayers[i].IsReady == false)
                isAllPlayersReady = false;
        }

        if (isAllPlayersReady)
            ContinueGameServerRpc();
    }

    [ClientRpc]
    private void PauseGameClientRpc()
    {
        Pause();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ContinueGameServerRpc()
    {
        for (int i = 0; i < _readyPlayers.Count; i++)
        {
            _readyPlayers[i] = new Player(_readyPlayers[i].Id, false);
        }

        Continue();

        ContinueGameClientRpc();
    }

    [ClientRpc]
    private void ContinueGameClientRpc()
    {
        Continue();
    }

    private void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0;

        OnStateChanged?.Invoke(_isPaused);
    }

    private void Continue()
    {
        _isPaused = false;
        Time.timeScale = 1;

        OnStateChanged?.Invoke(_isPaused);
    }
}
