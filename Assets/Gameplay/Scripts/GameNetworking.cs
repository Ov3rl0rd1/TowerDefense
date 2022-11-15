using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Linq;
using NaughtyAttributes;

public class GameNetworking : MonoBehaviour
{
    [SerializeField] private NetworkManager _debugNetworkManager;
    [SerializeField] private NetworkManager _buildNetworkManager;

    [Scene, SerializeField] private string _setupScene;
    [Scene, SerializeField] private string _gameScene;
    [Scene, SerializeField] private string _menuScene;

    private bool _isInGame;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != _setupScene)
            return;

        if (Application.isEditor)
            _debugNetworkManager.gameObject.SetActive(true);
        else
            _buildNetworkManager.gameObject.SetActive(true);

        SceneManager.LoadScene(_menuScene);
        DontDestroyOnLoad(gameObject);
    }

    public void StartHost()
    {
        if (_isInGame)
            throw new System.InvalidOperationException("Player is already in the game");

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.OnClientConnectedCallback += (e) => OnSecondPlayerConnected();
        NetworkManager.Singleton.OnClientDisconnectCallback += (e) => StopGame();

        _isInGame = true;
    }

    public void StartClient()
    {
        if (_isInGame)
            throw new System.InvalidOperationException("Player is already in the game");

        NetworkManager.Singleton.OnClientDisconnectCallback += (e) => StopGame();
        NetworkManager.Singleton.StartClient();

        _isInGame = true;
    }

    public void StopGame()
    {
        if (_isInGame == false)
            throw new System.InvalidOperationException("Player isn't in the game");

        _isInGame = false;
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);
        SceneManager.LoadScene(_setupScene);
    }

    private void OnSecondPlayerConnected()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(_gameScene, LoadSceneMode.Single);
    }
}
