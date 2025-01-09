using System.Collections;
using TMPro;
using UnityEngine;

public class CoinTextController : MonoBehaviour, IDataPersistence
{
    public int coinAmount;
    private TMP_Text coinText;


    private void Awake()
    {
        coinText = GetComponent<TMP_Text>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(DelayedStart());
        coinText.text = coinAmount.ToString();
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

    public void LoadData(GameData data)
    {
        this.coinAmount = data.coinAmount;
    }

    public void SaveData(GameData data)
    {
        data.coinAmount = this.coinAmount;
    }
}
