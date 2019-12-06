using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButtonOrb : MonoBehaviour
{
    public FinalBossCutscene finalBossCutscene;
    public MonoBehaviour target;
    private bool pressed = false;
    public GameObject surface, extraCollider;
    private Vector3 surfaceOrigPos;

    void Start()
    {
        surfaceOrigPos = surface.transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Orb" && !pressed)
        {
            pressed = true;

            extraCollider.SetActive(false);


            Debug.Log("MyButtonOrb -> Pressed");

            if (target is ActivatedBySwitchInterface a)
            {
                a.switchTurnedOn();
                Debug.Log("MyButtonOrb -> turnOn");
            }

            surface.transform.position = new Vector3(surfaceOrigPos.x, surfaceOrigPos.y - .8f, surfaceOrigPos.z);
            surface.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0);

            finalBossCutscene.OrbPress();
        }
    }


  
}
