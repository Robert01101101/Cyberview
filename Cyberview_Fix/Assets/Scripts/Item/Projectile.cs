using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
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

    public void SetupProjectile(float pLifetime, float pDamage, float pSpeed, bool pRight)
    {
        lifetime = pLifetime;
        damage = pDamage;
        speed = pSpeed;
        right = pRight;
        if (!right) { rightFactor = -1; } else { rightFactor = 1; }
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.layer == 12 || target.gameObject.tag=="rangedEnemy")
        {
            //Hit Enemy
            var script = target.gameObject.GetComponent<AbstractEnemy>();
            if (script != null)
            {
                script.HitBy(gameObject);
            }
        } else if (target.gameObject.tag == "DestroyableSceneObject")
        {
            target.gameObject.GetComponent<DestroyableSceneObject>().Hit();
        }
        Destroy(gameObject);
    }
}
