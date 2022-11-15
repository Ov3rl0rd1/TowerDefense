using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuLobbyUI : MonoBehaviour
{
    [SerializeField] private Button _host;
    [SerializeField] private Button _client;

    [Inject] private GameNetworking _gameNetworking;

    private void Awake()
    {
        _host.onClick.AddListener(() => _gameNetworking.StartHost());
        _client.onClick.AddListener(() => _gameNetworking.StartClient());
    }
}
