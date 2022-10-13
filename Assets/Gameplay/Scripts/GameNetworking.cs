using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Linq;

public class GameNetworking : MonoBehaviour
{
    private const string _setupScene = "Setup";
    private const string _gameScene = "Game";
    private const string _menuScene = "Menu";

    private static bool _isInGame;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != _setupScene)
            return;

        SceneManager.LoadScene(_menuScene);
        DontDestroyOnLoad(gameObject);
    }

    public static void StartHost()
    {
        if (_isInGame)
            throw new System.InvalidOperationException("Player is already in the game");

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.OnClientConnectedCallback += (e) => OnSecondPlayerConnected();
        NetworkManager.Singleton.OnClientDisconnectCallback += (e) => StopGame();

        _isInGame = true;
    }

    public static void StartClient()
    {
        if (_isInGame)
            throw new System.InvalidOperationException("Player is already in the game");

        NetworkManager.Singleton.OnClientDisconnectCallback += (e) => StopGame();
        NetworkManager.Singleton.StartClient();

        _isInGame = true;
    }

    public static void StopGame()
    {
        if (_isInGame == false)
            throw new System.InvalidOperationException("Player isn't in the game");

        _isInGame = false;
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);
        SceneManager.LoadScene(_setupScene);
    }

    private static void OnSecondPlayerConnected()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(_gameScene, LoadSceneMode.Single);
    }
}
