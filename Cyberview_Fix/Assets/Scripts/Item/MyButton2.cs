using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton2 : MonoBehaviour
{
    public GameObject surface;
    public MonoBehaviour target, target2;
    public bool pressed = false;
    public bool dontDeactivate = false;
    public int pressCount = 0;
    bool checkForExit;
    private Vector3 surfaceOrigPos;
    private Color unpressedColor;

    // Start is called before the first frame update
    void Start()
    {
        surfaceOrigPos = surface.transform.position;
        unpressedColor = surface.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (checkForExit && pressed)
        {
            checkForExit = false;
            StartCoroutine(ExitCheck());
        }
    }

    IEnumerator ExitCheck()
    {
        Vector3 checkPos = gameObject.transform.position;
        Vector2 size = new Vector2(6.56f, 2.27f);

        List<Collider2D> collidersAtCheckLocation = new List<Collider2D>(Physics2D.OverlapBoxAll(checkPos, size, 0));
        for (int i = collidersAtCheckLocation.Count - 1; i >= 0; i--)
        {
            if (!(collidersAtCheckLocation[i].gameObject.name == "_Player" || collidersAtCheckLocation[i].gameObject.tag == "HeavyBlock")) {
                collidersAtCheckLocation.Remove(collidersAtCheckLocation[i]); 
            }
        }
        if (collidersAtCheckLocation.Count == 0 && !dontDeactivate)
        {
            //no object on collider
            if (target is ActivatedBySwitchInterface a)
            {
                a.switchTurnedOff();
                pressed = false;
                //Debug.Log("lower call1");
            }
            if (target2 != null)
            {
                if (target2 is ActivatedBySwitchInterface a2)
                {
                    a2.switchTurnedOff();
                }
            }

            surface.transform.position = surfaceOrigPos;
            surface.GetComponent<SpriteRenderer>().color = unpressedColor;
        }
        //Debug.Log("collidersAtCheckLocation.Count" + collidersAtCheckLocation.Count);
        yield return new WaitForSeconds(.5f);
        if (collidersAtCheckLocation.Count != 0) checkForExit = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" || collision.gameObject.tag == "HeavyBlock")
        {
            checkForExit = true;
            if (target is ActivatedBySwitchInterface a)
            {
                a.switchTurnedOn();
                pressed = true;
            }
            if (target2 != null)
            {
                if (target2 is ActivatedBySwitchInterface a2)
                {
                    a2.switchTurnedOn();
                }
            }

            surface.transform.position = new Vector3(surfaceOrigPos.x, surfaceOrigPos.y - .8f, surfaceOrigPos.z);
            surface.GetComponent<SpriteRenderer>().color = new Color(1,0,0);
        }
    }
}
