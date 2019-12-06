using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairRoomCutscene : MonoBehaviour
{
    private PlayerManager playerManager;
    private DialogueHandler dialogueHandler;
    private bool delayedHealthCheck = true;
    public GameObject human, humanDialogue, findGunDialogue;
    private bool collected = false;
    public BodyModSwappingStation bmStn;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetPlayer());
    }

    IEnumerator GetPlayer()
    {
        yield return new WaitForSeconds(.5f);
        playerManager = GameObject.Find("_Player").GetComponent<PlayerManager>();
        dialogueHandler = GameObject.Find("DialogueHandler").GetComponent<DialogueHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerManager != null && delayedHealthCheck) StartCoroutine(DelayedHealthCheck());
    }

    IEnumerator DelayedHealthCheck()
    {
        delayedHealthCheck = false;
        yield return new WaitForSeconds(.5f);
        if (bmStn.chargeUsed)
        {
            StartCoroutine(Cutscene());
        } else
        {
            delayedHealthCheck = true;
        }
    }

    IEnumerator Cutscene()
    {
        humanDialogue.SetActive(false);

        playerManager.disableInputs = true;
        yield return new WaitForSeconds(.1f);
        dialogueHandler.showDialogue(AvatarShown.MINEMAN, "Unit 241, now that you are repaired, you are to report back to the mining floor.");
        yield return new WaitForSeconds(2.7f);
        dialogueHandler.showDialogue(AvatarShown.PROGAGONIST, "But... I almost just died down there! I'm scared to go back.");
        yield return new WaitForSeconds(2.7f);
        dialogueHandler.showDialogue(AvatarShown.MINEMAN, "Scared? You are supposed to follow our orders, you dumb robot!");
        yield return new WaitForSeconds(2.7f);
        dialogueHandler.showDialogue(AvatarShown.PROGAGONIST, "I don't want to, I'm scared!");
        yield return new WaitForSeconds(1.7f);
        dialogueHandler.showDialogue(AvatarShown.MINEMAN, "What??");
        yield return new WaitForSeconds(1f);
        dialogueHandler.showDialogue(AvatarShown.MINEMAN, "Oh no, the unit's intelligence inhibitor must be broken!!");
        yield return new WaitForSeconds(2.5f);
        dialogueHandler.showDialogue(AvatarShown.MINEMAN, "Everybody, get out of here, we have a disobedient bot!");
        yield return new WaitForSeconds(2f);
        dialogueHandler.hideDialogue();

        human.SetActive(false);
        playerManager.disableInputs = false;

        yield return new WaitForSeconds(3f);
        dialogueHandler.showDialogue(AvatarShown.MINEMAN, "All humans please evacuate to the ground floor. Code Yellow. This is not a drill. We have a rogue android. Please evacuate immediately. All Security Bots, engage rogue Unit 241.");
        yield return new WaitForSeconds(5f);
        dialogueHandler.hideDialogue();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && !collected)
        {
            collected = true;
            StartCoroutine(CollectedGun());
        }
    }

    IEnumerator CollectedGun()
    {
        findGunDialogue.SetActive(false);
        dialogueHandler.showDialogue(AvatarShown.PROGAGONIST, "Alright, I have a gun now. Now I need to equip it.");
        yield return new WaitForSeconds(4f);
        dialogueHandler.hideDialogue();
    }
}
