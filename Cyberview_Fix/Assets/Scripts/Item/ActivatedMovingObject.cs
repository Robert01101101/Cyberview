using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Note:  Also check MovingBridge.cs for an example of how to use a timer to tell an object when to stop moving

//Syntax for implementing the interface.  Make sure the base class (in this case, MonoBehaviour) is the first after the colon
//  Then the remaining are read as interfaces (of which there can be as many as we want)
public class ActivatedMovingObject : MonoBehaviour, ActivatedBySwitchInterface
{
    //Lets this object know if it has been activated or not
    public bool activated = false;
    //The speed that the object moves at when activated
    public float xSpeed = 0f;
    public float ySpeed = 0f;
    //the rigidbody for speed adjustments
    public Rigidbody2D body2D;
    
    //make sure that there is actually a Rigidbody2D component!
    void Start(){
        body2D = GetComponent<Rigidbody2D>();
    }


    //The button can't know about the internals of what it is activating, so the moving object needs to handle its own state
    //When activated, the object will move based on the speed settings
    //When not activated, the object will stop moving
    void Update()
    {
        if(activated){
            body2D.velocity = new Vector2(xSpeed, ySpeed);
        }
        else{
            //if the object is not activated, set its speed to 0 if it isn't already
            if(body2D.velocity.magnitude > 0){
                body2D.velocity = Vector2.zero;
            }
        }
    }

    //Need to provide definitions for the interface functions
    //Note it is not necessary to actually give them functionality, but even empty functions need to be defined
    //  In the MovingBridge example, un-pressing the switch still calls the function BUT since the function is empty it doesn't do anything
    //  This is useful if the object is not supposed to respond to the player letting go of the button
    
    //When the button is pressed, what should happen?
    //  Since the object needs to do something for an indefinite period of time, that behaviour is placed in the Update()
    //  To enable that functionality, we just need to activate the object
    public void switchTurnedOn(){
        activated = true;
    }

    //When the button is un-pressed, what should happen?
    //  The object needs to be un-activated
    public void switchTurnedOff(){
        activated = false;
    }
}
