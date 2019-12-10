using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL_0_MineCollapse : AbstractLvlItem
{
    //Hardcoded sequence to handle the mine collapse stuff.

    public GameObject theDudeThatRunsAway, rubble, bouldersToRemove, dialogue1, dialogue2, npc1, npc2, npc3;
    private bool collected;
    private AudioSource collapseSound;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MineAlreadyCollapsed")){
            theDudeThatRunsAway.SetActive(false);
            bouldersToRemove.SetActive(false);
            dialogue1.SetActive(false);
            dialogue2.SetActive(false);
            rubble.SetActive(true);
            collected = true;
        }
        collapseSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && !collected)
        {
            collected = true;
            StartCoroutine(MySequence());
        }
    }

    IEnumerator MySequence()
    {
        PlayerManager playerManager = GameObject.Find("_Player").GetComponent<PlayerManager>();
        HUD hud = GameObject.Find("_HUD").GetComponent<HUD>();
        GameObject.Find("DoorToMainArea").GetComponent<Door>().isPermanentlyLocked = true;
        GameObject.Find("DialogueHandler").GetComponent<DialogueHandler>().showDialogue(AvatarShown.MINEMAN, "Oh no! The mine is collapsing!! I better get out of here");
        theDudeThatRunsAway.SetActive(false);

        yield return new WaitForSeconds(2);
        GameObject.Find("DialogueHandler").GetComponent<DialogueHandler>().hideDialogue();

        yield return new WaitForSeconds(1);
        hud.BlackOutFX(4.5f);
        collapseSound.Play();

        playerManager.disableInputs = true;
        bouldersToRemove.SetActive(false);
        rubble.SetActive(true);
        playerManager.health = 25;
        PlayerPrefs.SetInt("PlayerHealth", 25);
        hud.SetHealth(25);
        hud.ShowTmpMsg("ERROR. ERROR. ERROR.", 4f);
        hud.PlayerHitFX();

        yield return new WaitForSeconds(1);
        hud.GetComponent<HUD>().PlayerHitFX();
        npc1.SetActive(false);
        npc2.SetActive(false);
        npc3.SetActive(false);

        yield return new WaitForSeconds(3.0f);
        playerManager.ChangeEyes();
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("DoorToMainArea").GetComponent<Door>().isPermanentlyLocked = false;
        hud.GetComponent<HUD>().ShowTmpMsg("Error: Critical System Damage. Go to repair floor immediately.", 4f);

        PlayerPrefs.SetInt("MineAlreadyCollapsed", 1);
    }
}
