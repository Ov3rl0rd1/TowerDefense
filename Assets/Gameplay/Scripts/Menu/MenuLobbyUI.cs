using UnityEngine;
using UnityEngine.UI;

public class MenuLobbyUI : MonoBehaviour
{
    [SerializeField] private Button _host;
    [SerializeField] private Button _client;

    private void Awake()
    {
        _host.onClick.AddListener(() => GameNetworking.StartHost());
        _client.onClick.AddListener(() => GameNetworking.StartClient());
    }
}
