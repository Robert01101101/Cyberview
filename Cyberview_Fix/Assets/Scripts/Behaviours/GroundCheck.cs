using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    //Kinda sloppy rn. Is used by both the player and enemies. Triggers are used for enemies, collisions are used for player (because
    //the enemy uses trigger colliders, while the player uses one solid collider for ground-check)

    public AbstractCharacter abstractCharacter;
    public bool groundCheckIsSolid = false;
    public bool isEnemyGroundCheck = false;
    private bool reversing = false;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == 8 || collision.gameObject.layer == 14 || collision.gameObject.tag == "rangedEnemy" || 
            collision.gameObject.tag == "DestroyableSceneObject") && groundCheckIsSolid) 
        {
            abstractCharacter.SetIsGrounded(true, gameObject.name);
            //Debug.Log("GroundCheck -> grounded");
        }
            
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == 8 || collision.gameObject.layer == 14 || collision.gameObject.tag == "rangedEnemy" || 
            collision.gameObject.tag == "DestroyableSceneObject") && groundCheckIsSolid)
        {
            abstractCharacter.SetIsGrounded(false, gameObject.name);
            //Debug.Log("GroundCheck -> not grounded");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 8 || collision.gameObject.tag == "DestroyableSceneObject" || collision.gameObject.tag == "Boulder") && !groundCheckIsSolid)
        {
            abstractCharacter.SetIsGrounded(true, gameObject.name);
        }
        if (isEnemyGroundCheck && (collision.gameObject.layer == 12 || collision.gameObject.tag == "rangedEnemy")) //(collided with another enemy)
        {
            abstractCharacter.SetIsGrounded(true, gameObject.name);
            //collision.gameObject.GetComponent<AbstractEnemy>().SetIsGrounded(true, gameObject.name);
            reversing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 8 || collision.gameObject.tag == "DestroyableSceneObject" || collision.gameObject.tag == "Boulder") && !groundCheckIsSolid)
        {
            abstractCharacter.SetIsGrounded(false, gameObject.name);
        }
        if (isEnemyGroundCheck && (collision.gameObject.layer == 12 || collision.gameObject.tag == "rangedEnemy")) //(collided with another enemy)
        {
            abstractCharacter.SetIsGrounded(false, gameObject.name);
            //collision.gameObject.GetComponent<AbstractEnemy>().SetIsGrounded(false, gameObject.name);
            reversing = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 8 || collision.gameObject.tag == "DestroyableSceneObject" || collision.gameObject.tag == "Boulder") && !groundCheckIsSolid)
        {
            abstractCharacter.SetIsGrounded(true, gameObject.name);
        }
        if (isEnemyGroundCheck && (collision.gameObject.layer == 12 || collision.gameObject.tag == "rangedEnemy")) //(collided with another enemy)
        {
            abstractCharacter.SetIsGrounded(true, gameObject.name);
            //collision.gameObject.GetComponent<AbstractEnemy>().SetIsGrounded(true, gameObject.name);
            reversing = true;
        }
    }
}
