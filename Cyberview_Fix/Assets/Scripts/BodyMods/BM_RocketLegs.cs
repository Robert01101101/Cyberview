using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_RocketLegs : AbstractBodyMod
{

    //Handles jumping. Perhaps we can create a Legs abstract class and a number of subclasses for each type of leg in the feature, so that
    //common functionality for jumps is shared.

    private bool jump;
    private Rigidbody2D rb;
    private PlayerManager playerManager;
    private bool flying = false;

    public float verticalAccel = 1f;
    public float maxVerticalSpeed = 30f;
    private float fallMultiplier = 2.5f; //sets factor of speed at which player falls back down
    private float lowJumpMultiplier = 1.5f; //sets factor of how hard low jumps break while going up (should be >1)(1 = low and high jumps are same)

    private int energyUsageCounter = 0;
    public int framesBeforeLosingEnergy = 10;

    void Update()
    {
        if (macroState == BodyModState.ACTIVE)
        {
            //(sloppy) set references if not set yet
            if (rb == null || playerManager == null)
            {
                rb = owner.gameObject.GetComponent<Rigidbody2D>();
                playerManager = owner.gameObject.GetComponent<PlayerManager>();

            }
            else
            {
                //Jump
                if (//jump && playerManager.isGrounded && !startingUp
                flying
                )
                {
                    float ySpeed = Mathf.Min(rb.velocity.y + verticalAccel, maxVerticalSpeed);
                    //rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                    rb.velocity = new Vector2(rb.velocity.x, ySpeed);
                    owner.GetComponentInChildren<Animator>().SetBool("jump", true);
                    //owner.GetPlayerSound().SoundJump();
                    energyUsageCounter++;
                    if(energyUsageCounter >= framesBeforeLosingEnergy){
                        owner.DecreaseHealth(energyCostPerTick, false);
                        energyUsageCounter = 0;
                    }

                    //start delay coroutine to avoid multiple calls while still grounded
                    //StartCoroutine(StartUpDelay());
                }


                //better jump physics during jump
                //if (rb.velocity.y < -.5f)
                //{
                //    rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                //}
                //else if (rb.velocity.y > .5f && !jump)
                //{
                //    rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
                //}
            }
        }
        
    }

    public override void DisableBodyMod()
    {
        flying = false;
    }

    public override void EnableBodyMod()
    {
        flying = true;
    }

    public override void EquipBodyMod()
    {
        GotoState(BodyModState.ACTIVE);
    }

    public override void UnequipBodyMod()
    {
        GotoState(BodyModState.INACTIVE);
    }
}
