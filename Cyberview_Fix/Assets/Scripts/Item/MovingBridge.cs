using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBridge : MonoBehaviour, ActivatedBySwitchInterface
{
    public bool activated = false;
    public float actionTimer = 3f;
    public Rigidbody2D body2d;
    public int xSpeed = 0;
    public int ySpeed;

    void Start(){
        body2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //check if need to do anything
        if(activated){
            if(actionTimer > 0){
                //do something
                //body2d.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                body2d.velocity = new Vector2(xSpeed, ySpeed);
                //body2d.constraints = RigidbodyConstraints2D.FreezeAll;
                actionTimer -= Time.deltaTime;
            }
            else{
                //do nothing
                if(body2d.velocity.magnitude > 0){
                    body2d.velocity = Vector2.zero;
                }
            }
        }
    }

    public void switchTurnedOn(){
        activated = true;
    }

    public void switchTurnedOff(){
        //do nothing
    }
}
