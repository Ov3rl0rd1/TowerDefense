using System;
using System.Collections;
using UnityEngine;

public class PlayerBalance : MonoBehaviour
{
    public static PlayerBalance Instance { get; private set; }

    public event Action OnBalanceChanged;

    public float Coins { get; private set; }
    public float EXP { get; private set; }
    public float BaseEfficiency => _coinGeneratorionCount;

    [SerializeField] private float _startCoins = 750;
    [SerializeField] private float _baseCoinGenerationCount = 5;
    [SerializeField] private float _baseCoinGeneratorSpeed = 2;

    private float _coinGeneratorionCount;

    private IEnumerator _coinGenerator;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("There are two PlayerBalance on the scene!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Coins = _startCoins;
        OnBalanceChanged?.Invoke();
        _coinGeneratorionCount = _baseCoinGenerationCount;
    }

    public void ReduceCoins(float value)
    {
        if (Coins < value || value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        Coins -= value;
        OnBalanceChanged?.Invoke();
    }

    public void IncreaseEXP(float value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        EXP += value;
        OnBalanceChanged?.Invoke();
    }

    public void ReduceEXP(float value)
    {
        if (EXP < value || value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        EXP -= value;
        OnBalanceChanged?.Invoke();
    }

    public void MultiplyGeneratorEfficiency(float value)
    {
        _coinGeneratorionCount = Mathf.Max(_coinGeneratorionCount * value, 0);
    }

    public void StartGenerator()
    {
        if (_coinGenerator != null)
            return;

        _coinGenerator = CoinGenerator();
        StartCoroutine(_coinGenerator);
    }

    public void StopGenerator()
    {
        if (_coinGenerator == null)
            return;

        StopCoroutine(_coinGenerator);
        _coinGenerator = null;
    }

    private IEnumerator CoinGenerator()
    {
        yield return new WaitForSeconds(_baseCoinGeneratorSpeed);

        Coins += _coinGeneratorionCount;
        OnBalanceChanged?.Invoke();

        _coinGenerator = CoinGenerator();
        StartCoroutine(_coinGenerator);
    }
}
