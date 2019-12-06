using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBehaviour : MonoBehaviour
{
    public Buttons[] inputButtons;
    protected InputState inputState;
    protected Rigidbody2D body2d;
    //protected CollisionState collisionState;

    protected virtual void Awake(){
        inputState = GetComponent<InputState>();
        body2d = GetComponent<Rigidbody2D>();
        //collisionState = GetComponent<CollisionState>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
