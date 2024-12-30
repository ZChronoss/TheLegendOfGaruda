using Unity.VisualScripting;
using UnityEngine;

public class UpgradeHealthPoint : MonoBehaviour
{
    public void OnDestroy(){
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        playerHealth.IncreaseHealthPoint(1);
    }
}
