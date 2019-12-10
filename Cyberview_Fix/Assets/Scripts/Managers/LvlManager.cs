using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class LvlManager : MonoBehaviour
{
    // Level Manager is placed once per level. GameManager looks for Level Manager on Level Load. It then calls InitLevel, so all initial
    // setting up for a level should be done in InitLevel. Other functionality includes time challenge, spawning rewards (so we can possibly
    // limit type of rewards for certain levels if we want), setting up camera.

    private GameObject defaultSpawnPoint; //used to spawn player if not coming form another level

    GameObject player;
    GameManager gameManager;
    private HUD hud;
    PlayerManager playerManager;

    private List<AbstractLvlItem> abstractLvlItems;
    private List<AbstractEnemy> abstractEnemy;

    float levelStartTime;
    private DoorKey[] doorKeyArray;
    private Door[] doorArray;
    int keysCollected = 0;
    public GameObject[] rewardArray;

    [System.NonSerialized]
    public int enemyCasualties, coinsCollected;

    private CinemachineVirtualCamera cinemachine;

    public bool repairRoom;

    public void InitLevel(GameManager gameManager, string lastSceneName, bool playerDied)
    {
        this.gameManager = gameManager;
        player = gameManager.player;
        playerManager = player.GetComponent<PlayerManager>();

        defaultSpawnPoint = GameObject.Find("SpawnPoint");
        hud = GameObject.Find("_HUD").GetComponent<HUD>();
        doorKeyArray = Object.FindObjectsOfType<DoorKey>();
        doorArray = Object.FindObjectsOfType<Door>();

        enemyCasualties = 0;
        coinsCollected = 0;

        //Find All Abstract Level Items & Enemies in order to reset level state
        abstractLvlItems = new List<AbstractLvlItem>(FindObjectsOfType<AbstractLvlItem>());
        abstractEnemy = new List<AbstractEnemy>(FindObjectsOfType<AbstractEnemy>());

        //filter to only items in level & disable if collected
        for (int i = abstractLvlItems.Count - 1; i >= 0; i--)
        {
            AbstractLvlItem ali = abstractLvlItems[i];
            if (ali.gameObject.scene.name != gameObject.scene.name)
            {
                abstractLvlItems.Remove(ali);
            } else
            {
                ali.DisableItemIfCollected();
            }
        }
        //filter to only enemies in level & disable if killed
        for (int i = abstractEnemy.Count - 1; i >= 0; i--)
        {
            AbstractEnemy aenemy = abstractEnemy[i];
            if (aenemy.gameObject.scene.name != gameObject.scene.name)
            {
                abstractEnemy.Remove(aenemy);
            }
            else
            {
                aenemy.DisableEnemyIfDead();
            }
        }
        if (playerDied)
        {
            //Re-Activate all Abstract Level Items in the scene
            foreach (AbstractLvlItem ali in abstractLvlItems)
            {
                ali.ReenableItem();
            }
            //Re-Activate all Abstract Level Items in the scene
            foreach (AbstractEnemy aenemy in abstractEnemy)
            {
                aenemy.ReenableEnemy();
            }

            //Recharge all body mod stations
            List<BodyModSwappingStation> bodyModStations = new List<BodyModSwappingStation>(FindObjectsOfType<BodyModSwappingStation>());
            foreach (BodyModSwappingStation bmstn in bodyModStations)
            {
                bmstn.LevelReset();
            }

            //Debug.Log("LvlManager Abstract Level Items Count: " + abstractLvlItems.Count);
        }



        Debug.Log("LvlManager -> Came from Scene : " + lastSceneName);
        if (lastSceneName == "") Debug.Log("WARNING: LvlManager -> lastScene is null");

        //clear HUD
        hud.HideTmpMsg();

        //figure out which Spawn Point to use
        if (lastSceneName != null)
        {

            //if coming from another level, override default spawn point with the door to that level
            foreach (Door door in doorArray){
                if (door.sceneToLoad == lastSceneName)
                {
                    defaultSpawnPoint = door.gameObject;
                    door.SetJustSpawned();
                }
            }
        }

        Debug.Log("LvlManager -> Using Spawn Point : " + defaultSpawnPoint.name);

        // set player pos
        player.transform.position = defaultSpawnPoint.transform.position;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        //playerManager.Recharge(50);
        playerManager.isFacingRight = true;

        //setup camera target
        cinemachine = GameObject.Find("Cinemachine Controller").GetComponent<CinemachineVirtualCamera>();
        cinemachine.Follow = player.transform;
        Debug.Log("LevelManager -> InitLevel");

        levelStartTime = Time.time;

        //Set saved states
    }

    public void CollectedDoorKey()
    {
        keysCollected++;
    }

    public void SpawnRandomReward(Vector2 rewardSpawnPoint)
    {
        int randomIdx = (int)(Random.Range(0, rewardArray.Length));
        //(because random is inclusive for max value)
        if (randomIdx == rewardArray.Length) randomIdx = rewardArray.Length - 1;
        
        Instantiate(rewardArray[randomIdx], rewardSpawnPoint, Quaternion.identity);
    }

    public void EnemyKilled()
    {
        enemyCasualties++;
    }

    public void CoinCollected()
    {
        coinsCollected++;
    }

    private void Update()
    {
        if (cinemachine.Follow == null) cinemachine.Follow = player.transform;
    }
}
