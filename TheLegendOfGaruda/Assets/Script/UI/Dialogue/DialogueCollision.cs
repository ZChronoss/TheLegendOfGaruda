using System;
using UnityEngine;

public class DialogueCollision : MonoBehaviour, IDataPersistence
{
    [SerializeField] private String id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    public DialogueTrigger trigger;
    private Boolean hasTriggered = false; // To ensure the dialogue triggers only once

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            if (trigger != null)
            {
                trigger.StartDialogue();
                Debug.Log("Dialogue started.");
            }
        }
    }

    public void LoadData(GameData data)
    {
        data.dialogues.TryGetValue(id, out hasTriggered);
        if (hasTriggered) 
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.dialogues.ContainsKey(id))
        {
            data.dialogues.Remove(id);
        }
        data.dialogues.Add(id, hasTriggered);
    }
}
