using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseUI : MonoBehaviour
{
    [Inject] private PauseManager _pauseManager;

    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _readyButton;
    [SerializeField] private Button _unreadyButton;
    [SerializeField] private float _readySwitchDealy = 5f;

    private IEnumerator _delayCoroutine;

    private void Awake()
    {
        _readyButton.onClick.AddListener(SetReady);
        _unreadyButton.onClick.AddListener(SetUnready);

        _pauseManager.OnStateChanged.AddListener(OnPauseStateChanged);
    }

    public void Pause()
    {
        _pauseManager.PauseGameServerRpc();

        _delayCoroutine = DelayCoroutine();
        StartCoroutine(_delayCoroutine);
    }

    private void SetReady()
    {
        if (_delayCoroutine != null)
            return;

        _pauseManager.SetReadyToContinue(true);
        _unreadyButton.gameObject.SetActive(true);
        _readyButton.gameObject.SetActive(false);

        _delayCoroutine = DelayCoroutine();
        StartCoroutine(_delayCoroutine);
    }

    private void SetUnready()
    {
        if (_delayCoroutine != null)
            return;

        _pauseManager.SetReadyToContinue(false);
        _unreadyButton.gameObject.SetActive(false);
        _readyButton.gameObject.SetActive(true);

        _delayCoroutine = DelayCoroutine();
        StartCoroutine(_delayCoroutine);
    }

    private void OnPauseStateChanged(bool isPaused)
    {
        _pausePanel.SetActive(isPaused);
        _unreadyButton.gameObject.SetActive(false);
        _readyButton.gameObject.SetActive(true);
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSecondsRealtime(_readySwitchDealy);

        _delayCoroutine = null;
    }
}
