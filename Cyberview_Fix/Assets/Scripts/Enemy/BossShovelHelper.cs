using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShovelHelper : MonoBehaviour
{
    public FinalBoss finalBoss;
    public BossAnimationHelper bossAnimationHelper;
    [System.NonSerialized]
    public int damageToPlayerPerHit;

    private void Start()
    {
        damageToPlayerPerHit = finalBoss.damageToPlayerPerHit;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            collision.gameObject.GetComponent<PlayerManager>().HitByEnemy(gameObject);
        }
        else if (collision.gameObject.tag == "HeavyBlock" && collision.gameObject.name != "BlockColliderHelper")
        {
            //ensure player is not holding the box
            PlayerManager playerManager = GameObject.Find("_Player").GetComponent<PlayerManager>();
            if (playerManager.bm_StrongArm.GetBox() != null)
            {
                Debug.Log("Playerbox:" + playerManager.bm_StrongArm.GetBox());
                Debug.Log("Bossbox:" + collision.gameObject);

                if (playerManager.bm_StrongArm.GetBox() != collision.gameObject)
                {
                    //grab box
                    bossAnimationHelper.GrabBox(collision.gameObject);
                    Debug.Log("GrabBox (custom)");
                }
            } else
            {
                //grab box
                bossAnimationHelper.GrabBox(collision.gameObject);
                Debug.Log("GrabBox");
            }
        }
    }
}
