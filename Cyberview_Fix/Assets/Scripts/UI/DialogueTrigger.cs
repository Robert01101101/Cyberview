using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    //Display a dialogue for as long as the player is within the area of the trigger

    private bool collected;
    [TextArea]
    public string message;
    public AvatarShown avatar;
    public bool displayOnlyOnce;
    private bool displayed;
    public bool showAfterCollapseOnly;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && !collected && (!displayOnlyOnce || !displayed))
        {
            GameObject handler = GameObject.Find("DialogueHandler");
            if (handler != null)
            {
                if (!showAfterCollapseOnly || collision.gameObject.GetComponent<PlayerManager>().health < 50) handler.GetComponent<DialogueHandler>().showDialogue(avatar, message);
            }
            collected = true;
            displayed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            GameObject handler = GameObject.Find("DialogueHandler");
            if (handler != null)
            {
                if (handler.GetComponent<DialogueHandler>().GetCurrentMessage() == message) handler.GetComponent<DialogueHandler>().hideDialogue();
            }
            collected = false;
        }
    }
}
