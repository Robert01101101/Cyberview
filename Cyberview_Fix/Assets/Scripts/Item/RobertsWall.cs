using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobertsWall : MonoBehaviour, ActivatedBySwitchInterface
{
    public bool activated = false;
    private Vector3 origPos, raisedPos;
    private Transform myTransform;
    private float speed;
    public bool finalWall;

    private void Start()
    {
        myTransform = GetComponent<Transform>();
        origPos = myTransform.position;
        raisedPos = new Vector3(origPos.x, origPos.y-27, origPos.z);
        if (finalWall) raisedPos = new Vector3(origPos.x, origPos.y - 40, origPos.z);
        if (finalWall) { speed = 1.5f; } else { speed = 30f; }
    }

    public void switchTurnedOff()
    {
        activated = false;
        //Debug.Log("lower");
    }

    public void switchTurnedOn()
    {
        activated = true;
        //Debug.Log("raise");
    }

    // Update is called once per frame
    void Update()
    {
        if (activated && myTransform.position.y != raisedPos.y)
        {
            StopAllCoroutines();
            StartCoroutine(MoveWall(raisedPos));
        } else if (!activated && myTransform.position.y != origPos.y)
        {
            StopAllCoroutines();
            StartCoroutine(MoveWall(origPos));
        }
    }

    IEnumerator MoveWall(Vector3 goToPos)
    {
        Vector3 vecA = myTransform.position;
        Vector3 vecB = goToPos;

        //floorAvatar.position 
        float step = (speed / (vecA - vecB).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            myTransform.position = Vector3.Lerp(vecA, vecB, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        myTransform.position = vecB;
    }
}
