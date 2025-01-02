using UnityEngine;

public class UpgradeHealthPotion : MonoBehaviour, IUpgradable
{
    public void Upgrade(){
        PlayerHealthPotion playerHealth = FindFirstObjectByType<PlayerHealthPotion>();
        playerHealth.IncreasePotionAmount(1);
    }
}
