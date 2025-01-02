using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int coinAmount;

    public int maxHealth;
    public int healthAmount;

    public int maxHPotion;
    public int hPotionAmount;

    public Vector3 playerPosition;

    public SerializableDictionary<string, bool> collectibles;
    public SerializableDictionary<string, bool> upgradables;
    public SerializableDictionary<string, bool> dialogues;

    // value yang di define di constructor bakal jadi default value di new game
    // (kalo gaada data yang bisa di load)
    public GameData()
    {
        this.coinAmount = 0;
        this.playerPosition = Vector3.zero;

        this.maxHealth = 3;
        this.healthAmount = 3;

        this.maxHPotion = 2;
        this.hPotionAmount = 2;

        collectibles = new SerializableDictionary<string, bool>();
        upgradables = new SerializableDictionary<string, bool>();
        dialogues = new SerializableDictionary<string, bool>();
    }
}
