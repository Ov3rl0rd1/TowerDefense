using UnityEngine;

[CreateAssetMenu(fileName = "CoinGeneratorLevel", menuName = "Data/Upgrades/Coin Generator", order = 0)]
public class PlayerBalanceUpgrade : BaseUpgrade
{
    public float Multiplier => _multiplier;

    [SerializeField] private float _multiplier;
}
