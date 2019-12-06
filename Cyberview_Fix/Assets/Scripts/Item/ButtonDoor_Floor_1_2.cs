using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor_Floor_1_2 : MonoBehaviour, ActivatedBySwitchInterface
{
    public bool activated = false;
    public int direction = 1; //positive up, negative down
    public float ySpeed = 25f;
    public float yPosStart, yPosEnd;
    private Rigidbody2D body2D;

    // Start is called before the first frame update
    void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(activated){
            direction = -1;
        }
        else{
            direction = 1;
        }

        if(direction < 0 && transform.position.y < yPosEnd){
            transform.position = new Vector2(transform.position.x, yPosEnd);
        }
        else if(direction > 0 && transform.position.y > yPosStart){
            transform.position = new Vector2(transform.position.x, yPosStart);
        }
        else{
            body2D.velocity = new Vector2(0, ySpeed * direction);
        }
    }

    public void switchTurnedOn(){
        activated = true;
    }

    public void switchTurnedOff(){
        activated = false;
    }
}
