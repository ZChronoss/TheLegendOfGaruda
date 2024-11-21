using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.VersionControl;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    private void Start()
    {
        StartDialogue();
    }
    public void StartDialogue() {
        FindFirstObjectByType<DialogueManager>().OpenDialogue(messages, actors);
    }
}

[System.Serializable]
public class Message {
    public int actorId;
    public string message;
}

[System.Serializable]
public class Actor {
    public string name;
    public Sprite sprite;
}