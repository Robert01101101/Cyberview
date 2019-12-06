using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : AbstractEnemy
{
    public FinalBossCutscene finalBossCutscene;
    GameObject playerGO;
    private bool playerInRange;
    private int speedXOffset = 1;
    private Vector3 origScale;
    public Animator animator;
    private ParticleSystem particles;
    public GameObject explosion, orb;

    private void Start()
    {
        origScale = gameObject.transform.localScale;
        particles = GetComponent<ParticleSystem>();
    }

    /*
    protected override void Update()
    {
        if (updateMovement) UpdateMovement();
    }*/

    public override void UpdateMovement()
    {
        //Debug.Log("FinalBoss -> UpdateMovement");

        //Move to player
        if (playerInRange)
        {
            float curSpeed = speed * speedXOffset;
            if (playerGO.transform.position.x > gameObject.transform.position.x) { speedXOffset = 1; } else { speedXOffset = -1; }
            body2d.velocity = new Vector2(curSpeed, body2d.velocity.y);

            //Flip Visually
            if (curSpeed > 0) { gameObject.transform.localScale = origScale; } 
            else { gameObject.transform.localScale = new Vector3(-origScale.x, origScale.y, origScale.z); }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            if (playerGO == null) playerGO = collision.gameObject;
            playerInRange = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player")
        {
            playerInRange = false;
        }
    }

    public void HitPlayer()
    {
        if (updateMovement)
        {
            StartCoroutine(WaitBeforeHit());
        }
    }

    IEnumerator WaitBeforeHit()
    {
        updateMovement = false;
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("hitPlayer", true);
        StartCoroutine(HitPlayerDelay());
    }

    IEnumerator HitPlayerDelay()
    {
        yield return new WaitForSeconds(3f);
        updateMovement = true;
    }

    public void PlayerLeft()
    {
        animator.SetBool("hitPlayer", false);
    }

    public void Damage()
    {
        health--;
        animator.SetBool("damage", true);
        particles.Clear();
        particles.Play();
        StartCoroutine(DamageAnimationDelay());
    }

    IEnumerator DamageAnimationDelay()
    {
        yield return new WaitForSeconds(.5f);
        animator.SetBool("damage", false);
    }

    public override void EnemyDeathStart()
    {
        StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation()
    {
        updateMovement = false;
        animator.SetBool("death", true);
        yield return new WaitForSeconds(3);
        Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        EnemyDeathEnd();
    }

    public override void EnemyDeathEnd()
    {
        LvlManager lvlManager = GameObject.Find("LevelManager").GetComponent<LvlManager>();

        //Spawn Reward
        lvlManager.SpawnRandomReward(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y-5.8f, gameObject.transform.position.z));
        lvlManager.SpawnRandomReward(new Vector3(gameObject.transform.position.x-2.5f, gameObject.transform.position.y - 5.8f, gameObject.transform.position.z));
        lvlManager.SpawnRandomReward(new Vector3(gameObject.transform.position.x+2.5f, gameObject.transform.position.y - 5.8f, gameObject.transform.position.z));
        lvlManager.SpawnRandomReward(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 4f, gameObject.transform.position.z));
        lvlManager.SpawnRandomReward(new Vector3(gameObject.transform.position.x - 2.5f, gameObject.transform.position.y - 4f, gameObject.transform.position.z));
        lvlManager.SpawnRandomReward(new Vector3(gameObject.transform.position.x + 2.5f, gameObject.transform.position.y - 4f, gameObject.transform.position.z));

        //Spawn Orb
        Instantiate(orb, gameObject.transform.position, Quaternion.identity);

        //cutscene stuff
        finalBossCutscene.BossDied();

        //save state
        PlayerPrefs.SetInt(objectID, 1);
        lvlManager.EnemyKilled();

        Destroy(gameObject);

    }

}
