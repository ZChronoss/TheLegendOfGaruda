using System;
using UnityEngine;

public class Coin : MonoBehaviour, IItem, IDataPersistence
{
    [SerializeField] private String id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public static event Action<int> OnCoinCollect;
    public int amount = 5;
    private Boolean collected = false;

    public void Collect()
    {
        OnCoinCollect.Invoke(amount);
        Destroy(gameObject);
        collected = true;
    }

    public void LoadData(GameData data)
    {
        data.collectibles.TryGetValue(id, out collected);
        if (collected) 
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.collectibles.ContainsKey(id))
        {
            data.collectibles.Remove(id);
        }
        data.collectibles.Add(id, collected);
    }
}
