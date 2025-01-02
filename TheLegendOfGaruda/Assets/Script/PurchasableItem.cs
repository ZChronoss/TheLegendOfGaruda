using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PurchasableItem : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid(){
        id = System.Guid.NewGuid().ToString();
    }
    public int cost = 10; // Cost of the item in coins
    public string itemName = "Health Upgrade";
    public string itemDescription = "Raises maximum health by 1 point";
    public GameObject floatingTextBox;
    public TMP_Text itemText;
    public TMP_Text itemCostText;
    private GameController playerCoins;
    private bool collected = false;

    private void Start(){
        if(!collected){
            floatingTextBox.SetActive(false);
            itemText.text = itemDescription;
            itemCostText.text = $"Cost: {cost} coins";
            playerCoins = FindFirstObjectByType<GameController>();
        }else{
            Destroy(gameObject);
        }
    }

    public void PurchaseItem()
    {
        if (playerCoins.coinAmount >= cost)
        {
            playerCoins.DecreaseCoinAmount(cost);
            collected = true;
            Destroy(gameObject);
        }
    }

    public void LoadData(GameData data){
        data.upgradables.TryGetValue(id, out collected);
        if(collected){
            Destroy(gameObject);
        }
    }

    public void SaveData(ref GameData data){
        if(data.upgradables.ContainsKey(id)){
            data.upgradables.Remove(id);
        }
        data.upgradables.Add(id, collected);
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
