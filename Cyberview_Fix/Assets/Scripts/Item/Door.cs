using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    // To use doors: place door prefab in scene. If you wish to use a key, place the key prefab as well and set it as the
    // DoorKey variable of the door game object.

    // Doors transport the player to the level specified as a string (use the scene name) via the sceneToLoad field.

    // For correct setup, place a door in that level that leads back to the origin level. For example: Door A in level 1 leads
    // to door B in level 2 and door B in level 2 leads back to Door A in level 1. That way the player spawns at the correct position.

    // If you wish to prevent the player from traveling back, simply set the door in the new level (door B in the example) to be
    // permanently locked.

    // PUBLIC
    public string sceneToLoad;
    public DoorKey doorKey;
    public bool isPermanentlyLocked;
    public bool isDoorToNextFloor;
    public bool doorToRepairRoom;
    public int originFloor;
    public int destinationFloor;
    private bool loadingScene = false;
    private bool justSpawned = false;
    private PlayerManager playerManager;
    private DialogueHandler dialogueHandler;

    private void Start()
    {
        if (isPermanentlyLocked) GetComponent<SpriteRenderer>().color = new Color(0.6889412f, 0.910423f, 0.9568627f);
        StartCoroutine(DelayedGrabDialogueHandlerRoutine());
    }

    IEnumerator DelayedGrabDialogueHandlerRoutine()
    {
        yield return new WaitForSeconds(.2f);
        dialogueHandler = GameObject.Find("DialogueHandler").GetComponent<DialogueHandler>();
        if (doorToRepairRoom) { if (!PlayerPrefs.HasKey("MineAlreadyCollapsed")) isPermanentlyLocked = true;  }
        if (isPermanentlyLocked) GetComponent<SpriteRenderer>().color = new Color(0.6889412f, 0.910423f, 0.9568627f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && !justSpawned && !isPermanentlyLocked)
        {
            PlayerManager playerManager = collision.gameObject.GetComponent<PlayerManager>();

            if (doorKey == null || playerManager.HasKey(doorKey))
            {
                GameObject.Find("_HUD").GetComponent<HUD>().ShowTmpMsg("Press TAB to open", 3);
            }
            else
            {
                GameObject.Find("_HUD").GetComponent<HUD>().ShowTmpMsg("Door is locked. Find the Key.", 3.5f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && !isPermanentlyLocked && justSpawned)
        {
            justSpawned = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && Input.GetKeyDown(KeyCode.Tab)){
            playerManager = collision.gameObject.GetComponent<PlayerManager>();

            string doorKeyID;
            string playerHasKeyDebug;
            if (doorKey != null) { doorKeyID = doorKey.objectID; if (playerManager.HasKey(doorKey)) { playerHasKeyDebug= "true"; } else { playerHasKeyDebug= "false"; } } else { doorKeyID = "null"; playerHasKeyDebug = "null"; }
            Debug.Log("Door Debug: -- isPermanentlyLocked: " + isPermanentlyLocked + ", -- loadingScene: " + loadingScene + ", -- doorKey: " + doorKeyID + ", -- playerManager.HasKey(doorKey): " + playerHasKeyDebug);
        }
        if (collision.gameObject.name == "_Player" && Input.GetKeyDown(KeyCode.Tab) && !isPermanentlyLocked && !loadingScene)
        {
            if (playerManager == null) playerManager = collision.gameObject.GetComponent<PlayerManager>();
            //load Door's scene if the door is not locked or has been unlocked by collecting the relevant key
            if (doorKey == null || playerManager.HasKey(doorKey))
            {
                loadingScene = true;
                playerManager.GetPlayerSound().SoundDoor();
                dialogueHandler.hideDialogue();
                if (isDoorToNextFloor) { StartCoroutine(FloorEndDelay()); } else { playerManager.gameManager.LoadScene(sceneToLoad); }
            }

        }
    }

    IEnumerator FloorEndDelay()
    {
        GameObject.Find("_HUD").GetComponent<HUD>().FinishedFloor(originFloor, destinationFloor);
        yield return new WaitForSeconds(6);
        if (isDoorToNextFloor) { playerManager.gameManager.newFloor = true; } else { playerManager.gameManager.newFloor = false; }
        playerManager.gameManager.LoadScene(sceneToLoad);
    }

    public void SetJustSpawned()
    {
        justSpawned = true;
    }

}
