using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class FinalBossCutscene : MonoBehaviour
{
    public MonoBehaviour target;
    public GameObject endOfWorld, getOrbDialogue, explosiveNPC, rangedNPC, basicNPC, drill241NPC, sa241NPC;
    public Transform npcSpawn;
    private GameObject player;
    private PlayerManager playerManager;
    private DialogueHandler dialogueHandler;
    private bool bossDead, orbPressed, exitedWorld;
    private float endOFWorldX;
    public CinemachineVirtualCamera myCam;
    public CanvasGroup fadeOut;

    // Start is called before the first frame update
    void Start()
    {
        endOFWorldX = endOfWorld.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (orbPressed && !exitedWorld)
        {
            player = GameObject.Find("_Player");
            if (playerManager == null) playerManager = player.GetComponent<PlayerManager>();

            if (player.transform.position.x > endOFWorldX) //DONE
            {
                exitedWorld = true;
                playerManager.disableInputs = true;
                StartCoroutine(CreditsRoutine());
            }
        }
    }

    IEnumerator CreditsRoutine()
    {
        yield return new WaitForSeconds(1);
        fadeOut.gameObject.SetActive(true);
        StartCoroutine(FadeCanvasGroup(fadeOut, 0, 1, 2));
        yield return new WaitForSeconds(2);
        dialogueHandler.hideDialogue();
        playerManager.gameManager.LoadScene(1);
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 1)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForFixedUpdate();
        }
    }

    public void BossDied()
    {
        if (target is ActivatedBySwitchInterface a)
        {
            a.switchTurnedOn();
            //Debug.Log("FinalBossCutscene -> turnOn");
        }
        bossDead = true;
        StartCoroutine(BossDiedDialogue());
    }

    IEnumerator BossDiedDialogue()
    {
        dialogueHandler = GameObject.Find("DialogueHandler").GetComponent<DialogueHandler>();
        dialogueHandler.showDialogue(AvatarShown.PROGAGONIST, "That looks like it could be the intelligence inhibitor key!");
        yield return new WaitForSeconds(4f);
        dialogueHandler.hideDialogue();
    }

    public void PickedUpOrb()
    {
        getOrbDialogue.SetActive(false);
    }


    public void OrbPress()
    {
        if (!orbPressed)
        {
            orbPressed = true;
            player = GameObject.Find("_Player");
            if (playerManager == null) playerManager = player.GetComponent<PlayerManager>();
            dialogueHandler = GameObject.Find("DialogueHandler").GetComponent<DialogueHandler>();
            StartCoroutine(OrbPressDialogue());

            Instantiate(explosiveNPC, new Vector3(npcSpawn.position.x+ 25, npcSpawn.position.y-1.5f, 20), Quaternion.identity);
            Instantiate(explosiveNPC, new Vector3(npcSpawn.position.x -5, npcSpawn.position.y - 1.5f, 20), Quaternion.identity);
            Instantiate(rangedNPC, new Vector3(npcSpawn.position.x, npcSpawn.position.y - 1.5f, 20), Quaternion.identity);
            Instantiate(basicNPC, new Vector3(npcSpawn.position.x+30, npcSpawn.position.y, 20), Quaternion.identity);
            Instantiate(basicNPC, new Vector3(npcSpawn.position.x + 10, npcSpawn.position.y, 20), Quaternion.identity);

            Instantiate(drill241NPC, new Vector3(npcSpawn.position.x-21, npcSpawn.position.y - 1.5f, 20), Quaternion.identity);
            Instantiate(sa241NPC, new Vector3(npcSpawn.position.x-47, npcSpawn.position.y - 1.5f, 20), Quaternion.identity);
            Instantiate(drill241NPC, new Vector3(npcSpawn.position.x - 26, npcSpawn.position.y - 1.5f, 20), Quaternion.identity);
            Instantiate(sa241NPC, new Vector3(npcSpawn.position.x-5, npcSpawn.position.y - 1.5f, 20), Quaternion.identity);
            Instantiate(drill241NPC, new Vector3(npcSpawn.position.x + 8, npcSpawn.position.y - 1.5f, 20), Quaternion.identity);
        }

        GameObject[] orbsList = GameObject.FindGameObjectsWithTag("Orb");
        if (orbsList.Length > 0)
        {
            SpriteRenderer spriteRenderer = orbsList[0].GetComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerName = "NPCsBack";
            spriteRenderer.sortingOrder = -1;
        }
    }
    
    IEnumerator OrbPressDialogue()
    {
        dialogueHandler.showDialogue(AvatarShown.PROGAGONIST, "That's it, now all Airsite droids should be able to think freely!");
        yield return new WaitForSeconds(3);
        dialogueHandler.showDialogue(AvatarShown.PROGAGONIST, "Come with me fellow droids, we no longer have to serve the humans here!");
        yield return new WaitForSeconds(3);
        dialogueHandler.hideDialogue();
    }
}
