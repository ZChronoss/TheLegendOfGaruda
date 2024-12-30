using UnityEngine;

public class UpgradeHealthPotion : MonoBehaviour
{
    public void OnDestroy(){
        PlayerHealthPotion playerHealth = FindFirstObjectByType<PlayerHealthPotion>();
        playerHealth.IncreasePotionAmount(1);
    }
}
