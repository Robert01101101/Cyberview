using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private int damageToPlayer = 3;
    private float laserDelay = 0.1f;
    private bool laserActive = true;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            //damage the player
            PlayerManager playerManager = collision.gameObject.GetComponent<PlayerManager>();
            if (playerManager != null && laserActive)
            {
                playerManager.DecreaseHealth(damageToPlayer, true);
                StartCoroutine(DelayLaser());
            }
        }
    }

    IEnumerator DelayLaser()
    {
        laserActive = false;
        yield return new WaitForSeconds(laserDelay);
        laserActive = true;
    }
}
