using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PurchasableItem : MonoBehaviour
{
    public int cost = 10; // Cost of the item in coins
    public string itemName = "Health Upgrade";
    public string itemDescription = "Raises maximum health by 1 point";
    public GameObject floatingTextBox;
    public TMP_Text itemText;
    public TMP_Text itemCostText;
    private GameController playerCoins;

    private void Start(){
        floatingTextBox.SetActive(false);
        itemText.text = itemDescription;
        itemCostText.text = $"Cost: {cost} coins";
        playerCoins = FindFirstObjectByType<GameController>();
    }

    public void PurchaseItem()
    {
        if (playerCoins.coinAmount >= cost)
        {
            playerCoins.DecreaseCoinAmount(cost);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            floatingTextBox.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            floatingTextBox.SetActive(false);
        }
    }
}
