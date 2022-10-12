using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using System.Linq;
using Zenject;

public class GameStateSwitcher : NetworkBehaviour
{
    public UnityEvent<Team> OnGameFinished;
    public GameState CurrentState => _currentState;

    private NetworkVariable<int> CurrentGameState = new NetworkVariable<int>();

    [SerializeField] private List<GameState> _states;
    [SerializeField] private Gates _player1Gates;
    [SerializeField] private Gates _player2Gates;

    [Inject] private PlayerBalance _playerBalance;

    private bool _isGameFinished;
    private GameState _currentState;

    private void Awake()
    {
        if (IsHost)
        {
            _player1Gates.OnDestroy.AddListener(EndGameServerRpc);
            _player2Gates.OnDestroy.AddListener(EndGameServerRpc);
        }
        else
            CurrentGameState.OnValueChanged += (p, n) => SetState(n);

        SetState(CurrentGameState.Value);
    }

    private void FixedUpdate()
    {
        if (_currentState.Timer(Time.fixedDeltaTime) <= 0)
        {
            if (IsHost)
            {
                if (CurrentGameState.Value + 1 == _states.Count)
                    EndGameServerRpc();
                else
                    SetState(CurrentGameState.Value + 1);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void EndGameServerRpc()
    {
        if (_isGameFinished)
            return;


        SetState(_states.Count - 1);
        _currentState.OnTimerEnd.AddListener(ReturnToLobby);
        _isGameFinished = true;

        Team winnerTeam = GetGameResult();
        OnGameFinished?.Invoke(winnerTeam);

        EndGameClientRpc(winnerTeam);
    }

    [ClientRpc]
    private void EndGameClientRpc(Team winnerTeam)
    {
        if (IsHost)
            return;

        SetState(_states.Count - 1);
        _currentState.OnTimerEnd.AddListener(ReturnToLobby);

        OnGameFinished?.Invoke(winnerTeam);

    }

    private void ReturnToLobby()
    {
        if (IsHost)
            NetworkManager.DisconnectClient(NetworkManager.ConnectedClientsIds.First(x => x != NetworkManager.LocalClientId));

        GameNetworking.StopGame();
    }

    private void SetState(int index)
    {
        CurrentGameState.Value = index;

        _currentState = _states[index];
        _currentState.StartState();

        if (_currentState.IsCoinGeneratorWork)
            _playerBalance.StartGenerator();
        else
            _playerBalance.StopGenerator();
    }

    private Team GetGameResult()
    {
        if (_isGameFinished == false)
            throw new System.InvalidOperationException("Game isn't over");

        Team winnerTeam = Team.None;

        if (_player1Gates.Health > _player2Gates.Health)
            winnerTeam = Team.First;
        else if (_player1Gates.Health < _player2Gates.Health)
            winnerTeam = Team.Second;

        return winnerTeam;
    }
}
