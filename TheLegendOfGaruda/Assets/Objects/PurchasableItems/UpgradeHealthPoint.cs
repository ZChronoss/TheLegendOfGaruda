using Unity.VisualScripting;
using UnityEngine;

public class UpgradeHealthPoint : MonoBehaviour, IUpgradable
{
    public void Upgrade(){
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        playerHealth.IncreaseHealthPoint(1);
    }
}
