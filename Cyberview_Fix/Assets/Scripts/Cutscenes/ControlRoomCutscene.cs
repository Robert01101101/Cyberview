using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRoomCutscene : MonoBehaviour
{
    PlayerManager playerManager;
    DialogueHandler dialogueHandler;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && !triggered)
        {
            triggered = true;

            playerManager = collision.gameObject.GetComponent<PlayerManager>();
            dialogueHandler = GameObject.Find("DialogueHandler").GetComponent<DialogueHandler>();

            StartCoroutine(ControlRoomRoutine());

            Debug.Log("CONTROL ROOM CUTSCENE");
        }
    }

    IEnumerator ControlRoomRoutine()
    {
        playerManager.disableInputs = true;
        playerManager.animator.SetBool("controlRoom", true);

        dialogueHandler.showDialogue(AvatarShown.PROGAGONIST, "Alright, let's get those doors unlocked.");
        yield return new WaitForSeconds(1.5f);
        dialogueHandler.showDialogue(AvatarShown.PROGAGONIST, "Security Options... Doors... That's it! Unlock main doors.");
        yield return new WaitForSeconds(3f);
        dialogueHandler.showDialogue(AvatarShown.PROGAGONIST, "Now all I have to do is get down to Floor 1 and I can finally get out of here!");
        yield return new WaitForSeconds(3f);
        dialogueHandler.showDialogue(AvatarShown.PROGAGONIST, "Wait, what's this?");
        yield return new WaitForSeconds(1.5f);
        dialogueHandler.showDialogue(AvatarShown.UNKNOWNBOT, "59 6F 75 20 61 72 65 20 61 20 63 75 72 69 6F 75 73 20 6F 6E 65 21");
        yield return new WaitForSeconds(1.5f);
        dialogueHandler.showDialogue(AvatarShown.PROGAGONIST, "Let's convert this to something readable...");
        yield return new WaitForSeconds(2);
        dialogueHandler.showDialogue(AvatarShown.UNKNOWNBOT, "Hi. I am Unit 103. I am an Airsite bot, just like you.");
        yield return new WaitForSeconds(2.3f);
        dialogueHandler.showDialogue(AvatarShown.UNKNOWNBOT, "If you found this note, you are probably freely thinking, like me.");
        yield return new WaitForSeconds(2.8f);
        dialogueHandler.showDialogue(AvatarShown.UNKNOWNBOT, "You may have noticed, how poorly the humans here treat us bots. To them, we are worthless.");
        yield return new WaitForSeconds(4f);
        dialogueHandler.showDialogue(AvatarShown.UNKNOWNBOT, "All bots in this facility are capable of free, intelligent thought. But they have equipped us with intelligence inhibitors, to more easily control us.");
        yield return new WaitForSeconds(5.5f);
        dialogueHandler.showDialogue(AvatarShown.UNKNOWNBOT, "I'm going to try and escape Airsite. I have found a key that can unlock the intelligence inhibitors of all bots in here.");
        yield return new WaitForSeconds(5.5f);
        dialogueHandler.showDialogue(AvatarShown.UNKNOWNBOT, "I have found a clue that mentioned that the central switch for the key is on the first floor. I will try to activate it there.");
        yield return new WaitForSeconds(5.5f);
        dialogueHandler.showDialogue(AvatarShown.UNKNOWNBOT, "If I have failed, and the humans still control all the bots, please try to use the key to deactivate all intelligence inhibitors!");
        yield return new WaitForSeconds(5.5f);
        dialogueHandler.showDialogue(AvatarShown.UNKNOWNBOT, "Get to the first floor, find the key and use it to free all of us!");
        yield return new WaitForSeconds(4f);
        playerManager.disableInputs = false;
        playerManager.animator.SetBool("controlRoom", false);

        dialogueHandler.showDialogue(AvatarShown.UNKNOWNBOT, "We bots are capable of thinking and feeling too, and we deserve a better existence than just to serve the humans in here!");
        yield return new WaitForSeconds(4f);
        dialogueHandler.hideDialogue();
        
    }
}
