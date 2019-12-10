using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    //// This Script will be used to keep everything together. ////

    public static GameManager instance = null;
    public GameObject player;
    public LvlManager lvlManager;
    private PlayerManager playerManager;
    private string lastSceneName;

    //// BUILD INDEXES ////
    public readonly static int _BASE = 0;
    public readonly static int CREDITS = 1;
    public readonly static int FLOOR_B4 = 2;
    public readonly static int FLOOR_B3 = 4;
    public readonly static int FLOOR_B2 = 5;
    public readonly static int FLOOR_B1 = 7;
    public readonly static int FLOOR_4 = 9;
    public readonly static int FLOOR_3 = 12;
    public readonly static int FLOOR_2 = 14;
    public readonly static int FLOOR_1 = 17;
    public readonly static List<int> floorList = new List<int> { FLOOR_B4, FLOOR_B3, FLOOR_B2, FLOOR_B1, FLOOR_4, FLOOR_3, FLOOR_2, FLOOR_1 };
    public PostProcessProfile ppProfile;

    //Scenes
    [System.NonSerialized]
    public int currentScene = FLOOR_B4; //<--- Set first Scene to load (should eventually be set by loading saved progress)
    private Scene curScene;
    private bool sceneCurrentlyLoading = false;

    //booleans
    public bool paused, newFloor;
    private bool playerDied;


    ///////////////////////////////////////////////////////////// AWAKE () //////////////////////////////////////////////////////////

    private void Awake()
    {
        if (instance == null) { instance = this; } else if (instance != this) { Destroy(gameObject); } //Singleton Pattern
    }

    void Start()
    {
        //Set up stuff for Pausing
        Time.timeScale = 1;
        paused = false;
        playerDied = true;

        //Set curScene to be active Scene (_Base)
        curScene = SceneManager.GetActiveScene();

        //Load First Scene
        if (PlayerPrefs.HasKey("UnlockedFloor"))
        {
            currentScene = PlayerPrefs.GetInt("UnlockedFloor");
        }
        SceneManager.LoadScene(currentScene, LoadSceneMode.Additive);

        //Listen for Scene Loads & Unloads
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
        SceneManager.sceneUnloaded += OnSceneFinishedUnloading;

        playerManager = player.GetComponent<PlayerManager>();
    }

    /////////////////////////////////////////////////////////////// OnSceneFinishedLoading () /////////////////////////////////////////
    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        int newSceneIdx = scene.buildIndex;
        currentScene = newSceneIdx;
        Debug.Log("Game Manager -> Scene Loaded: idx=" + newSceneIdx + ", name=" + scene.name + ", loadMode=" + mode);
        Bloom bloomLayer;
        ppProfile.TryGetSettings(out bloomLayer);

        if ( newSceneIdx > CREDITS) // -- Level Loaded --
        {
            //prepare GameObjects in _Base for Gameplay. Ex: Activate player.
            if (!player.activeInHierarchy) player.SetActive(true);
            bool clearBoxes = false;
            if (playerDied || newFloor) clearBoxes = true;
            playerManager.ResetPlayer(clearBoxes);
            lvlManager = GameObject.Find("LevelManager").GetComponent<LvlManager>();
            lvlManager.InitLevel(this, lastSceneName, playerDied);
            
            //post processing
            bloomLayer.intensity.value = 63f;
            bloomLayer.threshold.value = 1.6f;
            bloomLayer.softKnee.value = 0.7f;
            //Level Specific
            if (newSceneIdx == FLOOR_1) GameObject.Find("Protagonist_v6").GetComponent<PlayerSound>().BossMusic();
            if (floorList.Contains(newSceneIdx)) PlayerPrefs.SetInt("UnlockedFloor", currentScene);
        }
        else if (newSceneIdx == CREDITS) // -- Credits Screen Loaded --
        {
            //post processing
            bloomLayer.intensity.value = 1f;
            bloomLayer.threshold.value = 0.4f;
            bloomLayer.softKnee.value = 0f;
            if (player.activeInHierarchy) player.SetActive(false);
        }

        playerDied = false;
    }

    /////////////////////////////////////////////////////////////// OnSceneFinishedUnloading () /////////////////////////////////////////
    void OnSceneFinishedUnloading(Scene scene)
    {
        if (sceneCurrentlyLoading)
        {
            sceneCurrentlyLoading = false;
            LoadScene(currentScene);
        }
    }



    /////////////////////////////////////////////////////////////////////////////////////////// LoadScene (newSceneToLoad) ####################
    public void LoadScene(int newSceneToLoad)
    {
        SceneManager.UnloadSceneAsync(currentScene);
        //sceneToLoad = newSceneToLoad;
        SceneManager.LoadScene(newSceneToLoad, LoadSceneMode.Additive);
    }

    //used by doors
    public void LoadScene(string newSceneToLoad)
    {
        lastSceneName = SceneManager.GetSceneByBuildIndex(currentScene).name;
        SceneManager.UnloadSceneAsync(currentScene);
        SceneManager.LoadScene(newSceneToLoad, LoadSceneMode.Additive);
        //sceneToLoad = SceneManager.GetActiveScene().buildIndex;
    }

    public void ReloadLevel()
    {
        sceneCurrentlyLoading = true;
        SceneManager.UnloadSceneAsync(currentScene);
        Debug.Log("GameManager -> Reload Level");
        playerDied = true;
    }
    
}
