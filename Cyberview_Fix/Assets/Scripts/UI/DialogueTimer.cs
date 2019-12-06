using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTimer : AbstractLvlItem
{
    // Show a Dialogue after waiting as long as showDelay specifies and display it as long as hideDelay specifies.

    private bool collected;
    public bool disableMovement;

    [TextArea]
    public string message;
    public float showDelay, hideDelay;
    public AvatarShown avatar;

    
    public bool reenableMovement2;
    [TextArea]
    public string message2;
    public AvatarShown avatar2;
    public float hideDelay2;

    
    public bool reenableMovement3;
    [TextArea]
    public string message3;
    public AvatarShown avatar3;
    public float hideDelay3;

    private int iteration = 0;
    private string currentMessage;

    private PlayerManager playerManager;
    private DialogueHandler dialogueHandler;

    private void Start()
    {
        currentMessage = message;
        StartCoroutine(DelayedGrabDialogueHandlerRoutine());
    }

    IEnumerator DelayedGrabDialogueHandlerRoutine()
    {
        yield return new WaitForSeconds(.2f);
        if (dialogueHandler == null) dialogueHandler = GameObject.Find("DialogueHandler").GetComponent<DialogueHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && !collected)
        {
            playerManager = collision.gameObject.GetComponent<PlayerManager>();
            collected = true;
            StartCoroutine(MyDelay(avatar, message, showDelay, hideDelay));
            if (disableMovement) playerManager.disableInputs = true;
        }
    }

    IEnumerator MyDelay(AvatarShown inputAvatar, string inputMessage, float inputShowDelay, float inputHideDelay)
    {
        if (dialogueHandler == null) dialogueHandler = GameObject.Find("DialogueHandler").GetComponent<DialogueHandler>();
        yield return new WaitForSeconds(inputShowDelay);
        dialogueHandler.showDialogue(inputAvatar, inputMessage);
        yield return new WaitForSeconds(inputHideDelay);

        iteration++;

        if (message2 != "" && iteration == 1)
        {
            StartCoroutine(MyDelay(avatar2, message2, 0, hideDelay2));
            if (reenableMovement2) playerManager.disableInputs = false;
            currentMessage = message2;
        }

        if (message3 != "" && iteration == 2)
        {
            StartCoroutine(MyDelay(avatar3, message3, 0, hideDelay3));
            if (reenableMovement3) playerManager.disableInputs = false;
            currentMessage = message3;
        }

        if (message2 == "" || (message2 != "" && iteration == 2) || (message3 != "" && iteration == 3))
        {
            //save state
            PlayerPrefs.SetInt(objectID, 1);
            if (dialogueHandler.GetCurrentMessage() == currentMessage)
            {
                dialogueHandler.hideDialogue();
            }
            if (disableMovement) playerManager.disableInputs = false;
        }
    }

    
}
