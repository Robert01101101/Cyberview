using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    // Handle all HUD stuff

    public TextMeshProUGUI armLValue, armRValue, legsValue, healthValue, creditValue, tmpMsg, enemyCasualties, coinsCollected, rechargeBtnLabel;
    public TMP_Dropdown armLDropdown, armRDropdown, legsDropdown;
    public PlayerManager playerManager;
    public GameManager gameManager;
    public CanvasGroup playerHUD, pauseMenu, deathMenu, floorEndScreen, floorEndOverlay, playerHitCG, bmMenu;
    public RawImage batteryBar, batteryBarOutline;
    public RectTransform batteryParent, floorAvatar;
    public GameObject blackout, enemyKillCount, debugBtn, debug2Btn, debug3Btn, debug4Btn;
    private float originalPlayerHitCGalpha, origBatterySizeX;
    private AudioSource clickSound;
    private bool bmMenuLoaded, bmFrameDelay, pulsing, goScreen, chargeUsed;
    private int originFloor, destinationFloor;
    public Button rechargeBtn;
    private BodyModSwappingStation bmStn;
    private Color floorTextDark = new Color(0.07843138f, 0.07843138f, 0.07843138f);
    private Color floorTextBright = new Color(1f, 0.56488f, 0.112f);

    private void Awake()
    {
        originalPlayerHitCGalpha = playerHitCG.alpha;
        origBatterySizeX = batteryBar.rectTransform.sizeDelta.x;
        playerHitCG.alpha = 0;
        clickSound = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1) { BtnPause(); } else { BtnResume(); }
        }
        if (Input.GetKeyDown(KeyCode.Tab) && bmMenuLoaded && !bmFrameDelay)
        {
            BtnExitBMMenu();
        }
        if (Input.GetKeyDown(KeyCode.Tab) && bmMenuLoaded && bmFrameDelay) bmFrameDelay = false;

        int health = playerManager.health;
        if (health <= 20)
        {
            if (!pulsing) StartCoroutine(BatteryPulse());
            healthValue.color = new Color(mapNumber(health, 0, playerManager.origHealth, 1f, 0.2f), mapNumber(health, 0, playerManager.origHealth, 0.2f, 0.8f), 0.2f);
            batteryBarOutline.color = new Color(mapNumber(health, 0, playerManager.origHealth, 1f, 0.2f), mapNumber(health, 0, playerManager.origHealth, 0.2f, 0.8f), 0.2f);
        } else
        {
            healthValue.color = new Color(1, 1,1);
            batteryBarOutline.color = new Color(0,0,0);
        }
        if (Time.timeScale == 0)
        {
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.H))
            {
                debugBtn.SetActive(true); debug2Btn.SetActive(true); debug3Btn.SetActive(true); debug4Btn.SetActive(true);
            }
        }
    }

    IEnumerator BatteryPulse()
    {
        pulsing = true;

        for (float t = 0.0f; t < 1.5f; t += Time.deltaTime / 1.5f)
        {
            float xScale, yScale;
            if (t < 0.75f)
            {
                xScale = mapNumber(t, 0, 0.75f, 1, 1.05f);
                yScale = mapNumber(t, 0, 0.75f, 1, 1.1f);
            }
            else
            {
                xScale = mapNumber(t, 0.75f, 1.5f, 1.05f, 1);
                yScale = mapNumber(t, 0.75f, 1.5f, 1.1f, 1);
            }
            batteryParent.localScale = new Vector2(xScale, yScale);
            yield return null;
        }

        pulsing = false;
    }

    //----------------------------------------------------------- OnClick Methods -------------------------------------------------

    public void BtnPause ()
    {
        debugBtn.SetActive(false); debug2Btn.SetActive(false); debug3Btn.SetActive(false); debug4Btn.SetActive(false);
        clickSound.Play();
        gameManager.paused = true;
        Debug.Log("HUD -> Pause");
        Time.timeScale = 0;
        playerHUD.alpha = 0;
        playerHUD.interactable = false;
        playerHUD.gameObject.SetActive(false);
        pauseMenu.alpha = 1;
        pauseMenu.interactable = true;
        pauseMenu.gameObject.SetActive(true);
    }

    public void BtnResume()
    {
        clickSound.Play();
        gameManager.paused = false;
        Debug.Log("HUD -> Resume");
        Time.timeScale = 1;
        playerHUD.alpha = 1;
        playerHUD.interactable = true;
        playerHUD.gameObject.SetActive(true);
        pauseMenu.alpha = 0;
        pauseMenu.interactable = false;
        pauseMenu.gameObject.SetActive(false);
    }

    public void BtnQuit()
    {
        Application.Quit();
        Debug.Log("HUD -> Quit");
    }

    public void BtnSkipLevel()
    {
        clickSound.Play();
        gameManager.paused = false;
        Debug.Log("HUD -> Skip Level");
        Time.timeScale = 1;
        playerHUD.alpha = 1;
        playerHUD.interactable = true;
        playerHUD.gameObject.SetActive(true);
        pauseMenu.alpha = 0;
        pauseMenu.interactable = false;
        pauseMenu.gameObject.SetActive(false);
        gameManager.LoadScene(gameManager.currentScene + 1);
    }

    public void BtnGround()
    {
        playerManager.isGrounded = true;
        Debug.Log("HUD -> Grounded");
    }

    public void BtnClearStorage()
    {
        clickSound.Play();
        Debug.Log("HUD -> DEBUG: Cleared Storage");
        PlayerPrefs.DeleteAll();
    }

    public void BtnUnlockAllDoors()
    {
        clickSound.Play();
        Debug.Log("HUD -> DEBUG: Unlocked all doors");
        List<Door> doors = new List<Door>(FindObjectsOfType<Door>());
        foreach(Door d in doors)
        {
            d.doorKey = null;
        }
    }

    public void BtnExitBMMenu()
    {
        clickSound.Play();
        gameManager.paused = false;
        Debug.Log("HUD -> Exit BM MEnu");
        Time.timeScale = 1;
        playerHUD.alpha = 1;
        playerHUD.interactable = true;
        playerHUD.gameObject.SetActive(true);
        bmMenu.alpha = 0;
        bmMenu.interactable = false;
        bmMenu.gameObject.SetActive(false);

        //Apply selected body mods
        playerManager.SetMod(0, GetDropdownBodyMod(armLDropdown));
        playerManager.SetMod(1, GetDropdownBodyMod(armRDropdown));
        playerManager.SetMod(2, GetDropdownBodyMod(legsDropdown));

        UpdateBodyModsDisplay();

        bmMenuLoaded = false;
    }

    public void BtnRestartLevel()
    {
        LvlManager lvlManager = GameObject.Find("LevelManager").GetComponent<LvlManager>();

        clickSound.Play();
        gameManager.paused = false;
        Debug.Log("HUD -> Restart");
        Time.timeScale = 1;
        playerHUD.alpha = 1;
        playerHUD.interactable = true;
        playerHUD.gameObject.SetActive(true);
        pauseMenu.alpha = 0;
        pauseMenu.interactable = false;
        pauseMenu.gameObject.SetActive(false);

        //if restarting from gameover screen
        if (goScreen)
        {
            deathMenu.alpha = 0;
            deathMenu.interactable = false;
            deathMenu.gameObject.SetActive(false);
            goScreen = false;
        }

        gameManager.ReloadLevel();
    }

    public void BtnRecharge()
    {
        playerManager.Recharge(100);
        bmStn.ChargeUsed();
        rechargeBtn.interactable = false;
        rechargeBtnLabel.text = "Additional Recharge: \n 25 Credits";
        rechargeBtnLabel.fontSize = 25;
        if (chargeUsed) playerManager.RechargeCoinsCharge();
    }

    //----------------------------------------------------------- Dropdown Logic -------------------------------------------------

    private void UpdateBodyModsDropdownOptions(TMP_Dropdown dropdown)
    {
        List<string> availableUpperBodyModsStrings = new List<string>();
        List<string> availableLowerBodyModsStrings = new List<string>();
        List<string> bmStrings = new List<string>();
        List<AbstractBodyMod> availableBodyMods = playerManager.GetUnlockedBodyMods();

        //check which body mods of which type are unlocked
        foreach (AbstractBodyMod abm in availableBodyMods)
        {
            if (abm.bodyModType == BodyModType.UPPERBODY) availableUpperBodyModsStrings.Add(abm.name);
            if (abm.bodyModType == BodyModType.LOWERBODY) availableLowerBodyModsStrings.Add(abm.name);
        }

        //list only applicable types (upper vs lower body)
        if (dropdown.gameObject.name== "ArmLDropdown" || dropdown.gameObject.name == "ArmRDropdown")
        {
            bmStrings = availableUpperBodyModsStrings;
        } else
        {
            bmStrings = availableLowerBodyModsStrings;
        }

        //clear/remove all option item
        dropdown.options.Clear();
        dropdown.options.Add(new TMP_Dropdown.OptionData() { text = "none" });

        int selectedItem = 0;
        int counter = 1;

        //fill the dropdown menu OptionData with all applicable Body Mod Names
        foreach (string c in bmStrings)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = c });

            //set the currently active dropdown (set to "none" if none is active)
            if (dropdown == armLDropdown) {
                if (playerManager.GetArmOneMod() != null) {
                    if (playerManager.GetArmOneMod().name == c) selectedItem = counter;
                } else
                {
                    selectedItem = 0;
                }
            }
            else if (dropdown == armRDropdown)
            {
                if (playerManager.GetArmTwoMod() != null)
                {
                    if (playerManager.GetArmTwoMod().name == c) selectedItem = counter;
                }
                else
                {
                    selectedItem = 0;
                }
            }
            else if (dropdown == legsDropdown)
            {
                if (playerManager.GetLegsMod() != null)
                {
                    if (playerManager.GetLegsMod().name == c) selectedItem = counter;
                }
                else
                {
                    selectedItem = 0;
                }
            }
            counter++;
        }

        //refresh Visuals by assigning a temporary new value
        dropdown.value = 1;
        dropdown.value = 0;
        //set active body mod as selected on the Dropdown
        dropdown.value = selectedItem;
    }

    private AbstractBodyMod GetDropdownBodyMod(TMP_Dropdown dropdown)
    {
        //check which option is selected in the dropdown menu and return the corresponding reference within the player
        AbstractBodyMod equippedBodyMod = null;
        if (dropdown.options[dropdown.value].text == playerManager.bm_Drill.name) equippedBodyMod = playerManager.bm_Drill;
        if (dropdown.options[dropdown.value].text == playerManager.bm_Gun.name) equippedBodyMod = playerManager.bm_Gun;
        if (dropdown.options[dropdown.value].text == playerManager.bm_StrongArm.name) equippedBodyMod = playerManager.bm_StrongArm;
        if (dropdown.options[dropdown.value].text == playerManager.bm_Legs.name) equippedBodyMod = playerManager.bm_Legs;
        if (dropdown.options[dropdown.value].text == playerManager.bm_SuperLegs.name) equippedBodyMod = playerManager.bm_SuperLegs;
        if (dropdown.options[dropdown.value].text == playerManager.bm_Grapple.name) equippedBodyMod = playerManager.bm_Grapple;
        if (dropdown.options[dropdown.value].text == playerManager.bm_RocketLegs.name) equippedBodyMod = playerManager.bm_RocketLegs;
        
        return equippedBodyMod;
    }

    //Update In-Game HUD Body Mod Display
    public void UpdateBodyModsDisplay()
    {
        if (playerManager.GetArmOneMod() != null)
        {
            armLValue.text = playerManager.GetArmOneMod().GetComponent<AbstractBodyMod>().name;
        } else { armLValue.text = "-"; }
        if (playerManager.GetArmTwoMod() != null)
        {
            armRValue.text = playerManager.GetArmTwoMod().GetComponent<AbstractBodyMod>().name;
        } else { armRValue.text = "-"; }
        if (playerManager.GetLegsMod() != null)
        {
            legsValue.text = playerManager.GetLegsMod().GetComponent<AbstractBodyMod>().name;
        } else { legsValue.text = "-"; }
    }

    //----------------------------------------------------------- Public Interface -------------------------------------------------

    public void InitializeHUD()
    {
        UpdateBodyModsDisplay();
        SetHealth(playerManager.GetHealth());
        creditValue.text = playerManager.GetCredit().ToString();
    }

    public void LoadBodyModMenu(bool chargeUsed, BodyModSwappingStation bmStn)
    {
        this.bmStn = bmStn;
        this.chargeUsed = chargeUsed;

        if (!bmMenuLoaded) {
            clickSound.Play();
            gameManager.paused = true;
            Debug.Log("HUD -> Body Mod Menu");
            Time.timeScale = 0;
            playerHUD.alpha = 0;
            playerHUD.interactable = false;
            playerHUD.gameObject.SetActive(false);
            bmMenu.alpha = 1;
            bmMenu.interactable = true;
            bmMenu.gameObject.SetActive(true);
            if (!chargeUsed) { 
                rechargeBtn.interactable = true;
                rechargeBtnLabel.text = "Recharge";
                rechargeBtnLabel.fontSize = 40;
            } else {
                if (playerManager.credit >= 25) {
                    rechargeBtn.interactable = true;
                } else
                {
                    rechargeBtn.interactable = false;
                }
                rechargeBtnLabel.text = "Additional Recharge: \n 25 Credits";
                rechargeBtnLabel.fontSize = 25;
            }

            UpdateBodyModsDropdownOptions(armLDropdown);
            UpdateBodyModsDropdownOptions(armRDropdown);
            UpdateBodyModsDropdownOptions(legsDropdown);

            bmMenuLoaded = true;
            bmFrameDelay = true;
        }   
    }

    public void PlayerDied()
    {
        if (!goScreen)
        {
            gameManager.paused = true;
            Debug.Log("HUD -> PlayerDied()");
            Time.timeScale = 0;
            playerHUD.alpha = 0;
            playerHUD.interactable = false;
            playerHUD.gameObject.SetActive(false);
            deathMenu.alpha = 1;
            deathMenu.interactable = true;
            deathMenu.gameObject.SetActive(true);

            goScreen = true;
        }
    }

    public void SetHealth(int health) {
        if (health < 0) health = 0;
        healthValue.text = health.ToString() + "%";
        batteryBar.rectTransform.sizeDelta = new Vector2(mapNumber(health, 0, playerManager.origHealth, 0, origBatterySizeX), 51.85f);
        batteryBar.color = new Color(mapNumber(health, 0, playerManager.origHealth, 1f, 0.2f), mapNumber(health, 0, playerManager.origHealth, 0.2f, 0.8f), 0.2f);
    }

    public void SetCredit(int credit) { creditValue.text = credit.ToString(); }

    public void ShowTmpMsg (string msg, float messageDur)
    {
        tmpMsg.text = msg;
        tmpMsg.gameObject.SetActive(true);
        StartCoroutine(TmpMsgDelay(messageDur));
    }

    public void HideTmpMsg()
    {
        tmpMsg.gameObject.SetActive(false);
    }

    IEnumerator TmpMsgDelay(float messageDur)
    {
        yield return new WaitForSeconds(messageDur);
        tmpMsg.gameObject.SetActive(false);
    }

    public void PlayerHitFX()
    {
        StartCoroutine(FadeCanvasGroup(playerHitCG, originalPlayerHitCGalpha, 0, 1.5f, false));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime, bool floorEnd)
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

            if (percentageComplete >= 1)
            {
                if (floorEnd)
                {
                    if (end == 1) { StartCoroutine(FloorEndScreenDelay()); 
                    } else {
                        floorEndScreen.interactable = false;
                        floorEndScreen.gameObject.SetActive(false);
                        floorEndOverlay.gameObject.SetActive(false);
                        floorEndOverlay.alpha = 0;
                    }
                }
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void BlackOutFX(float delay)
    {
        StartCoroutine(BlackoutDelay(delay));
    }

    IEnumerator BlackoutDelay(float delay)
    {
        blackout.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        blackout.gameObject.SetActive(false);
    }

    float mapNumber(float pX, float pA, float pB, float pM, float pN)
    {
        return (pX - pA) / (pB - pA) * (pN - pM) + pM;
    } 

    public void FinishedFloor(int originFloor, int destinationFloor)
    {
        this.originFloor = originFloor;
        this.destinationFloor = destinationFloor;

        playerHUD.alpha = 0;
        playerHUD.interactable = false;
        playerHUD.gameObject.SetActive(false);

        floorEndOverlay.alpha = 0;
        floorEndOverlay.gameObject.SetActive(true);

        int casualties = GameObject.Find("LevelManager").GetComponent<LvlManager>().enemyCasualties;
        enemyCasualties.text = casualties.ToString();
        if (casualties > 1) { enemyCasualties.color = new Color(.9f,.1f,.1f); } else { enemyCasualties.color = new Color(1f, 1f, 1f); }
        coinsCollected.text = GameObject.Find("LevelManager").GetComponent<LvlManager>().coinsCollected.ToString();

        playerManager.disableInputs = true;

        StartCoroutine(FadeCanvasGroup(floorEndOverlay, 0, 1f, 1f, true));
    }

    //TODO: floorEndOverlay

    IEnumerator FloorEndScreenDelay()
    {
        floorEndScreen.interactable = true;
        floorEndScreen.alpha = 0;
        floorEndScreen.gameObject.SetActive(true);

        StartCoroutine(MovePlayerAvatar());
        StartCoroutine(FadeCanvasGroup(floorEndScreen, 0f, 1f, 1f, false));
        yield return new WaitForSeconds(4.1f);
        StartCoroutine(FadeCanvasGroup(floorEndScreen, 1f, 0f, 1f, false));
        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeCanvasGroup(floorEndOverlay, 1f, 0f, 1f, true));

        playerHUD.alpha = 1;
        playerHUD.interactable = true;
        playerHUD.gameObject.SetActive(true);

        playerManager.disableInputs = false;
    }

    IEnumerator MovePlayerAvatar()
    {
        if (originFloor > 0) { enemyKillCount.SetActive(true); } else { enemyKillCount.SetActive(false); }

        string originGameObjectName = "AvatarP_A_" + originFloor.ToString();
        string destinationGameObjectName = "AvatarP_B_" + destinationFloor.ToString();

        TextMeshProUGUI originText = GameObject.Find("Floor_Label_" + originFloor.ToString()).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI destinationText = GameObject.Find("Floor_Label_" + destinationFloor.ToString()).GetComponent<TextMeshProUGUI>();

        for (int i=4; i>=(-4); i--)
        {
            if (i != 0) GameObject.Find("Floor_Label_" + i.ToString()).GetComponent<TextMeshProUGUI>().color = floorTextDark;
        }

        originText.color = floorTextBright;
        StartCoroutine(SwitchFloorTextColor(originText, destinationText));

        Vector3 vecA = GameObject.Find(originGameObjectName).GetComponent<RectTransform>().position;
        Vector3 vecB = GameObject.Find(destinationGameObjectName).GetComponent<RectTransform>().position;
        floorAvatar.position = vecA;

        yield return new WaitForSeconds(1.5f);
        
        int speedFactor = (int) (Vector3.Magnitude(vecA - vecB));

        //floorAvatar.position 
        float step = (1 * speedFactor / (vecA - vecB).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            floorAvatar.position = Vector3.Lerp(vecA, vecB, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        floorAvatar.position = vecB;
    }

    IEnumerator SwitchFloorTextColor(TextMeshProUGUI originText, TextMeshProUGUI destinationText)
    {
        yield return new WaitForSeconds(2f);
        destinationText.color = floorTextBright;
        originText.color = floorTextDark;
    }
}
