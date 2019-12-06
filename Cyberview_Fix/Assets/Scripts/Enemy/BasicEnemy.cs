using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : AbstractEnemy
{
    public BoxCollider2D leftFloorDetector;
    public BoxCollider2D rightFloorDetector;
    private Vector3 origScale, flippedScale;
    public Animator animator;

    //private bool groundCheckDelay = false;

    private void Start()
    {
        origScale = gameObject.transform.localScale;
        flippedScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void UpdateMovement()
    {
        //walk
        body2d.velocity = new Vector2(speed, body2d.velocity.y);
        //if (speed < 3 && speed > -3) Debug.Log("Basic Enemy -> speed = " + speed);
        if (updateMovement) { if (speed > 0) { gameObject.transform.localScale = origScale; } else { gameObject.transform.localScale = flippedScale; } }
    }

    public override void PlayerCollision(GameObject player)
    {
        if (updateMovement) StartCoroutine(HitThrowback(player, true));
        animator.SetBool("hitPlayer", true);
        if (player.transform.position.x > gameObject.transform.position.x) { gameObject.transform.localScale = origScale; } else { gameObject.transform.localScale = flippedScale; }
    }

    IEnumerator HitThrowback(GameObject weapon, bool playerCollision)
    {
        updateMovement = false;
        body2d.velocity = new Vector2(0, 0);
        //use varying force for weapon vs player hits
        if (!playerCollision) { body2d.AddForce(-(weapon.transform.position - gameObject.transform.position).normalized * 2000); }
        else { body2d.AddForce(-(weapon.transform.position - gameObject.transform.position).normalized * 1500); }
        //Debug.Log("Enemy -> HitThrowback");
        yield return new WaitForSeconds(0.5f);
        updateMovement = true;
        animator.SetBool("hitPlayer", false);
        if (speed > 0) { gameObject.transform.localScale = origScale; } else { gameObject.transform.localScale = flippedScale; }
    }

}
