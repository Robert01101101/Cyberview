using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : AbstractEnemy
{
    //issue here: how can I do damage to player?
    //is the player able to do damage to this enemy?
    //wont stop firing bullets
    //bullets wont get destroyed
   //public BoxCollider2D leftFloorDetector;
    //public BoxCollider2D rightFloorDetector;
    public Animator animator;
    private Transform player; //holds the enemies target
    public bool checkRight = true;
    [SerializeField]
    GameObject rangedBullets;
    public float shootDelay = 1;
    public float bulletLifetime = 3f;
    public float bulletDamage = 1;
    public float bulletSpeed = 10f;

    private bool canShoot = true;
    private float distanceFromPlayer = 3.2f;

    // Start is called before the first frame update


    void Start()
    {
    	player = GameObject.Find("_Player").GetComponent<Transform>();
        // bulletRate = 3f;
        // nextBullet = Time.time;
        updateMovement = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

      //If the player is farther than 15 away and NOT farther than 30 away, follow the player. Else, don't.
        if (Vector2.Distance(transform.position, player.position) > 5 && Vector2.Distance(transform.position, player.position) < 30) {
        	animator.SetBool("walk", true);
            if (canShoot)  StartCoroutine(ShootDelay());
        	transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
           
		if(player.position.x > transform.position.x && !checkRight) //if the target is to the right of enemy and the enemy is not facing right
		        lookAtPlayer(); //enemy is now facing correctly
              
		if(player.position.x < transform.position.x && checkRight) //if the target is to the left of enemy and the enemy is not facing left
		        lookAtPlayer();//enemy is now facing correctly

		} else {
			animator.SetBool("walk", false);
        }
    }

    private void Shoot()
    {
        Vector2 enemyPos = gameObject.transform.position;

        //account for direction the player is facing
        int xPosFactor;
        if (checkRight) { xPosFactor = 1; } else { xPosFactor = -1; }
        Vector2 spawnPos = new Vector2(transform.position.x + (distanceFromPlayer * xPosFactor)-2, transform.position.y+3);
        Vector2 size = new Vector2(.5f, .5f);

         //spawn bullet
       
            GameObject gunObject = Instantiate(rangedBullets, spawnPos, Quaternion.identity);
            Bullets Bullets = gunObject.GetComponent<Bullets>();

            //setup bullet properties
            Bullets.SetupBullet(bulletLifetime, bulletDamage, bulletSpeed, checkRight);
            //delay to avoid shooting once per frame
    }

      IEnumerator ShootDelay()
    {
        canShoot = false;
        Shoot();
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

     void lookAtPlayer(){
      Vector3 scale = transform.localScale;
      scale.x *= -1;
      transform.localScale = scale;
      checkRight = !checkRight;
    }
    public override void UpdateMovement()
    {
        //TODO: Improve logic to check whether enemy got stuck (for some reason doesn't always work)
        //if (body2d.IsSleeping()) { speed = -speed; Debug.Log("stuck"); }

        //walk
	     if (animator.GetBool("walk")){
	        body2d.velocity = new Vector2(speed, body2d.velocity.y);
	        //if (speed < 3 && speed > -3) Debug.Log("Ranged Enemy -> speed = " + speed);
	    } 
    }


}
/*
Citation
https://www.youtube.com/watch?v=rhoQd6IAtDo
https://stackoverflow.com/questions/53488507/unity-2d-rotate-the-ai-enemy-to-look-at-player
*/