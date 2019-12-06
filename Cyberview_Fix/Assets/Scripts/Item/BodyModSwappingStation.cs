using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyModSwappingStation : MonoBehaviour
{
    [System.NonSerialized]
    public bool chargeUsed = false;

    [System.NonSerialized]
    public string objectID;

    public SpriteRenderer glow;

    private void Awake()
    {
        objectID = gameObject.scene.name + ", x=" + gameObject.transform.position.x + ", y=" + gameObject.transform.position.y;
        if (PlayerPrefs.HasKey(objectID))
        {
            chargeUsed = true;
            glow.color = new Color(1f, 0f, 0.6313726f);
        } else
        {
            chargeUsed = false;
            glow.color = new Color(0f, 1f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            GameObject.Find("_HUD").GetComponent<HUD>().ShowTmpMsg("Press TAB to use Body Modding Station", 3f);
            //collision.gameObject.GetComponent<PlayerManager>().Recharge(100);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && Input.GetKeyDown(KeyCode.Tab))
        {
            HUD hud = GameObject.Find("_HUD").GetComponent<HUD>();
            hud.LoadBodyModMenu(chargeUsed, this);
            Debug.Log("charge used:" + chargeUsed);
        }
    }

    public void ChargeUsed()
    {
        chargeUsed = true;
        PlayerPrefs.SetInt(objectID, 1);
        glow.color = new Color(1f, 0f, 0.6313726f);
    }

    public void LevelReset()
    {
        chargeUsed = false;
        if (PlayerPrefs.HasKey(objectID)) PlayerPrefs.DeleteKey(objectID);
        glow.color = new Color(0f, 1f, 0f);
    }
}
