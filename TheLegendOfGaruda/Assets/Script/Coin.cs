using System;
using UnityEngine;

public class Coin : MonoBehaviour, IItem
{
    // basically ini kek subscriber gitu
    public static event Action<int> OnCoinCollect;
    public int amount = 5;

    public void Collect()
    {
        OnCoinCollect.Invoke(amount);
        Destroy(gameObject);
    }
}
