using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

public class StateTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _value;

    [Inject] private GameStateSwitcher _gameStateSwitcher;

    private void FixedUpdate()
    {
        _text.text = _gameStateSwitcher.CurrentState.TimerName;

        int minutes = (int)_gameStateSwitcher.CurrentState.TimerValue / 60;
        int seconds = (int)_gameStateSwitcher.CurrentState.TimerValue % 60;

        _value.text = $"{minutes}:{(seconds > 9 ? seconds : "0" + seconds)}";
    }
}
