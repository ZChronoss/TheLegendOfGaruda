using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Playables;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage;
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;
    public RectTransform backgroundBox;

    private Message[] currentMessages;
    private Actor[] currentActors;
    private int activeMessage = 0;
    public static bool isActive = false;

    public float typingSpeed = 0.05f;

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;

        Debug.Log("Started conversation, Loaded messages: " + messages.Length);
        backgroundBox.transform.localScale = Vector3.one;
        DisplayMessage();
    }

    public void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];

        // If the message has an associated timeline, play it and wait for completion
        if (messageToDisplay.timeline != null)
        {
            Debug.Log("Playing timeline for message...");
            messageToDisplay.timeline.Play();
            StartCoroutine(WaitForTimelineToFinish(messageToDisplay));
            backgroundBox.transform.localScale = Vector3.zero;
            return; // Do not display the message until the timeline is complete
        }
        backgroundBox.transform.localScale = Vector3.one;

        // Display actor and message
        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;

        // Start typing the message
        StopAllCoroutines(); // Ensure no other typing coroutine is running
        StartCoroutine(TypeLine(messageToDisplay.message));
    }

    private IEnumerator WaitForTimelineToFinish(Message message)
    {
        while (message.timeline.state == PlayState.Playing)
        {
            yield return null; // Wait until the timeline finishes
        }

        Debug.Log("Timeline finished.");

        message.timeline = null;
        
        NextMessage();
    }

    private IEnumerator TypeLine(string message)
    {
        messageText.text = ""; // Clear the text
        foreach (char letter in message.ToCharArray())
        {
            messageText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextMessage()
    {
        activeMessage++;
        if (activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            Debug.Log("Conversation ended");
            backgroundBox.transform.localScale = Vector3.zero;
            isActive = false;
        }
    }

    void Start()
    {
        backgroundBox.transform.localScale = Vector3.zero;
    }

    void Update()
    {
        // Check if dialogue is active and handle input
        if (currentMessages == null)
        {
            isActive = false;
        }

        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && isActive)
        {
            if (messageText.text == currentMessages[activeMessage].message)
            {
                NextMessage();
            }
            else
            {
                // Skip typing and display the full message immediately
                StopAllCoroutines();
                messageText.text = currentMessages[activeMessage].message;
            }
        }
    }
}
