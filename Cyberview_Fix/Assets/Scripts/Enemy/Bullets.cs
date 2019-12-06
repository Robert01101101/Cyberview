using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    private Rigidbody2D body2d;
    private float lifetime = 10f;
    private float damage = 1;
    private float speed = 10f;
    private bool right = true;
    private int rightFactor;

    private void Awake()
    {
        body2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        body2d.velocity = new Vector2(speed*rightFactor, 0);
    }

    public void SetupBullet(float pLifetime, float pDamage, float pSpeed, bool pRight)
    {
        lifetime = pLifetime;
        damage = pDamage;
        speed = pSpeed;
        right = pRight;
        if (!right) { rightFactor = -1; } else { rightFactor = 1; }
    }
    private void OnTriggerEnter2D(Collider2D target)
    {
        //Debug.Log("Bullet Collision: " + target.gameObject.name);

        if (target.gameObject.name == "_Player")
        {
            //damage the player
            var script = target.gameObject.GetComponent<PlayerManager>();
            if (script != null)
            {
                script.HitByEnemy(gameObject);
            }
            Destroy(gameObject);
        }
        else if (target.gameObject.tag != "rangedBullet" && target.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "HeavyBlock" || target.gameObject.tag == "DestroyableSceneObject")
        {
            Destroy(gameObject);
        }
    }

    public int DamageToCharacter()
    {
        return (int) damage;
    }
}













// 	float bulletSpeed = 7f;
// 	Rigidbody2D rigidbody2d;
// 	PlayerManager target; //holds the enemies target
// 	Vector2 moveHere;
//     // Start is called before the first frame update
//     void Start()
//     {
//     	rigidbody2d = GetComponent<Rigidbody2D>();
//     	target = GameObject.FindObjectOfType<PlayerManager>(); 
//     	moveHere = (target.transform.position - transform.position).normalized * bulletSpeed;
//     	rigidbody2d.velocity = new Vector2(moveHere.x, moveHere.y);
//     	Destroy (gameObject, 3f);
        
//     }

//     void OnTriggerEnter2D (Collider2D col){
//     	if (col.gameObject.name.Equals("PlayerManager")){
//     		Destroy (gameObject);
//     	}
//     }
//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }

