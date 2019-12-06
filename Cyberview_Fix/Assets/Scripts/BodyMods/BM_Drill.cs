using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_Drill : AbstractBodyMod
{
    //Takes care mostly of animations. Boulder digging logic is in -> Boulder.cs. The activation / deactivation of the drill
    //occurs by enabling / disabling the drill trigger collider that Boulder.cs checks for.

    Collider2D myCollider;
    private static float offsetX = 5f;
    private static AudioSource drillSound;
    bool checkingForBoulder = false;


    void Start()
    {
        myCollider = GetComponent<CircleCollider2D>();
        drillSound = GetComponent<AudioSource>();
        myCollider.enabled = false;
    }


    public override void EnableBodyMod()
    {
        if (!owner.strongArmsInUse)
        {
            if (myCollider == null) myCollider = GetComponent<CircleCollider2D>();

            myCollider.enabled = true;
            if (!checkingForBoulder)
            {
                //raise Arm & begin checking if it should be lowered again
                if (armSide == ArmSide.ARMTWO) { animator.SetBool("raiseArmL", true); } else { animator.SetBool("raiseArmR", true); }
                StartCoroutine(DelayedBoulderCheck());
            }
            checkingForBoulder = true;
        }
    }

    IEnumerator DelayedBoulderCheck()
    {
        //lower Arm if no boulder in range
        ContactFilter2D contactFilter = new ContactFilter2D();
        Collider2D[] colliderList = new Collider2D [10];
        Physics2D.OverlapCollider(myCollider, contactFilter, colliderList);
        bool hitBoulder = false;

        //check if collider has hit Boulder
        for (int i=0; i< colliderList.Length; i++)
        {
            if (colliderList[i] != null)
            {
                if (colliderList[i].gameObject.tag == "Boulder")
                {
                    hitBoulder = true;
                }
            }
        }
        if (hitBoulder && !startingUp)
        {
            GotoState(BodyModState.ACTIVE);
            if (!drillSound.isPlaying) drillSound.Play();
            yield return new WaitForSeconds(.5f);
        } else
        {
            drillSound.Stop();
            yield return new WaitForSeconds(.5f);
            //lower arm & delay again to let lower Arm animation finish
            if (armSide == ArmSide.ARMTWO) { animator.SetBool("raiseArmL", false); } else { animator.SetBool("raiseArmR", false); }
            yield return new WaitForSeconds(.3f);
        }
        checkingForBoulder = false;
    }

    public override void DisableBodyMod()
    {
        myCollider.enabled = false;
        if (armSide == ArmSide.ARMTWO) { animator.SetBool("raiseArmL", false); } else { animator.SetBool("raiseArmR", false); }
        GotoState(BodyModState.INACTIVE);
    }

    public override void EquipBodyMod()
    {
    }

    public override void UnequipBodyMod()
    {
    }

    public void BoulderDestroyed()
    {
        owner.DecreaseHealth(energyCostPerTick, false);
        drillSound.Stop();
    }
}
