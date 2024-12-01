using UnityEngine;

public class DialogueCollision : MonoBehaviour
{
    public DialogueTrigger trigger;
    private bool hasTriggered = false; // To ensure the dialogue triggers only once

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

}
