using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage;
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;
    public RectTransform backgroundBox;

    Message[] currentMessages;
    Actor[] currentActors;
    int activeMessage = 0;
    public static bool isActive = false;

    public float typingSpeed = 0.05f;

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;

        Debug.Log("Started conversation, Loaded messages: " + messages.Length);
        // backgroundBox.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo();
        backgroundBox.transform.localScale = Vector3.one;
        DisplayMessage();
    }

    public void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];

        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;

        // Start typing the message
        StopAllCoroutines(); // Ensure no other typing coroutine is running
        StartCoroutine(TypeLine(messageToDisplay.message));
    }

    IEnumerator TypeLine(string message)
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
            // backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            backgroundBox.transform.localScale = Vector3.zero;
            isActive = false;
        }
    }

    void Start()
    {
        backgroundBox.transform.localScale = Vector3.zero;
        Debug.Log(isActive);
    }


    void Update()
    {
        // Kasus dimana kalo user buka dialogue llu lsg tutup game
        if (currentMessages == null)
        {
            isActive = false;
        }
        //MouseButton for testing
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
