using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHelper : MonoBehaviour
{
    public FinalBoss finalBoss;
    private bool hitPlayer, delayedCheck;

    private void Start()
    {
        delayedCheck = true;
    }

    private void Update()
    {
        if (delayedCheck) StartCoroutine(DelayedCheck());
    }

    IEnumerator DelayedCheck()
    {
        delayedCheck = false;
        hitPlayer = false;
        yield return new WaitForSeconds(1);
        if (!hitPlayer) finalBoss.PlayerLeft();
        delayedCheck = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            hitPlayer = true;
            finalBoss.HitPlayer();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            hitPlayer = true;
            finalBoss.HitPlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            hitPlayer = false;
            finalBoss.PlayerLeft();
        }
    }
}
