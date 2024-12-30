using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int coinAmount;
    public TMP_Text coinText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coinAmount = 0;
        coinText.text = coinAmount.ToString();
        // Tiap kali coin ke collect, code ini jalan
        Coin.OnCoinCollect += IncreaseCoinAmount;
    }

    void IncreaseCoinAmount(int amount)
    {
        coinAmount += amount;
        coinText.text = coinAmount.ToString();
    }

    public void DecreaseCoinAmount(int amount)
    {
        coinAmount -= amount;
        coinText.text = coinAmount.ToString();
    }
}
