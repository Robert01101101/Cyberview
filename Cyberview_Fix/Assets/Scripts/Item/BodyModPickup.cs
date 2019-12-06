using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyModPickup : AbstractLvlItem
{
    //Check if Player picks up Body Mod Collectible. If that is the case, call PlayerManager's UnlockBodyMod(). Also display Message.

    public GameObject bodyModPrefab;
    private AbstractBodyMod bodyMod;

    void Start()
    {
        bodyMod = bodyModPrefab.GetComponent<AbstractBodyMod>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            collision.gameObject.GetComponent<PlayerManager>().UnlockBodyMod(bodyMod);
            GameObject.Find("_HUD").GetComponent<HUD>().ShowTmpMsg("New Body Mod unlocked: " + bodyMod.name, 6);

            //save state
            PlayerPrefs.SetInt(objectID, 1);

            Destroy(gameObject);
        }
    }

    public override void ReenableItem()
    {
        List<AbstractBodyMod> unlockedBodyMods = GameObject.Find("_Player").GetComponent<PlayerManager>().GetUnlockedBodyMods();
        bool containsMod = false;
        foreach(AbstractBodyMod abm in unlockedBodyMods)
        {
            if (abm.name == bodyModPrefab.GetComponent<AbstractBodyMod>().name) containsMod = true;
        }
        if (!containsMod)
        {
            gameObject.SetActive(true);
            if (PlayerPrefs.HasKey(objectID)) PlayerPrefs.DeleteKey(objectID);
        }
    }
}
