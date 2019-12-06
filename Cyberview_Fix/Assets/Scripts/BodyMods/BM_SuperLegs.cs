using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_SuperLegs : AbstractBodyMod
{

    //Handles jumping. Perhaps we can create a Legs abstract class and a number of subclasses for each type of leg in the feature, so that
    //common functionality for jumps is shared.

    private bool jump;
    private Rigidbody2D rb;
    private PlayerManager playerManager;

    private float jumpSpeed = 45f;
    private float fallMultiplier = 2.5f; //sets factor of speed at which player falls back down
    private float lowJumpMultiplier = 1.5f; //sets factor of how hard low jumps break while going up (should be >1)(1 = low and high jumps are same)

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
                if (jump && playerManager.isGrounded && !startingUp)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                    owner.GetComponentInChildren<Animator>().SetBool("jump", true);
                    owner.GetPlayerSound().SoundJump();
                    owner.DecreaseHealth(energyCostPerTick, false);

                    //start delay coroutine to avoid multiple calls while still grounded
                    StartCoroutine(StartUpDelay());
                }


                //better jump physics during jump
                if (rb.velocity.y < -.5f)
                {
                    rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                }
                else if (rb.velocity.y > .5f && !jump)
                {
                    rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
                }
            }
        }
        
    }

    public override void DisableBodyMod()
    {
        jump = false;
    }

    public override void EnableBodyMod()
    {
        jump = true;
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
