using UnityEngine;
using TMPro;
using Zenject;

public class GameResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameResultText;
    [SerializeField] private Color _winTextColor;
    [SerializeField] private Color _loseTextColor;
    [SerializeField] private Color _drawTextColor;

    [Inject] private GameStateSwitcher _gameState;
    [Inject] private Team _team;

    private void Awake()
    {
        _gameResultText.gameObject.SetActive(false);
        _gameState.OnGameFinished.AddListener(EnableText);
    }

    public void EnableText(Team winnerTeam)
    {
        _gameResultText.gameObject.SetActive(true);

        if (winnerTeam == _team)
            _gameResultText.color = _winTextColor;
        else
            _gameResultText.color = _loseTextColor;

        if (winnerTeam == Team.None)
        {
            _gameResultText.color = _drawTextColor;
            _gameResultText.text = "Draw";
        }
        else
            _gameResultText.text = $"{winnerTeam} team won!";
    }
}
