using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton : MonoBehaviour
{
    public MonoBehaviour target, target2;
    public bool pressed = false;
    public int pressCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == 9 || collision.gameObject.tag == "HeavyBlock") && 
            collision.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0.1 &&
            collision.gameObject.transform.position.y > transform.position.y + 2) 
        {
            if(target is ActivatedBySwitchInterface a && pressCount == 0){
                a.switchTurnedOn();
                pressed = true;
            }
            if (target2 is ActivatedBySwitchInterface a2 && pressCount == 0)
            {
                a2.switchTurnedOn();
                pressed = true;
            }
            pressCount += 1;
        }   
    }

    public void OnCollisionExit2D(Collision2D collision){
        if ((collision.gameObject.layer == 9 || collision.gameObject.tag == "HeavyBlock") && pressed) 
        {
            if(pressCount == 1 && target is ActivatedBySwitchInterface a){
                a.switchTurnedOff();
            }
            if (pressCount == 1 && target2 is ActivatedBySwitchInterface a2)
            {
                a2.switchTurnedOff();
            }
            pressCount -= 1;
            if(pressCount <= 0){
                pressed = false;
            }
        } 
    }
}
