using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameState", menuName = "Data/Game State")]
public class GameState : ScriptableObject
{
    [HideInInspector] public UnityEvent OnTimerEnd;

    public string TimerName => _timerName;
    public float TimerValue => _timer;
    public bool IsCoinGeneratorWork => _isCoinGeneratorWork;
    public bool CanSendUnits => _canSendUnits;

    [SerializeField] private string _timerName;
    [SerializeField] private bool _isCoinGeneratorWork;
    [SerializeField] private bool _canSendUnits;
    [SerializeField] private float _stateTimer;

    private float _timer;

    public void StartState()
    {
        _timer = _stateTimer;
    }

    public float Timer(float deltaTime)
    {
        _timer = Mathf.Max(_timer - deltaTime, 0);

        if (_timer == 0)
            OnTimerEnd?.Invoke();

        return _timer;
    }
}
