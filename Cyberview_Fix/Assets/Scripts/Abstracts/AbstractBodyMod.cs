using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This enum is intended to track macro-states that a body mod can be in
//INACTIVE -> body mod button is not pressed and therefore the body mod is doing nothing
//STARTUP -> the associated button has been pressed and any initial actions can begin now
//ACTIVE -> vaguely defined but should be used when the button is held.
//ENDLAG -> the associated button has been released and the body mod might have to do some cleanup
public enum BodyModState {
    INACTIVE,
    STARTUP,
    ACTIVE,
    ENDLAG
}

public enum BodyModType {
    UPPERBODY,
    LOWERBODY
}

public enum ArmSide
{
    ARMONE,
    ARMTWO,
    NEITHER
}

public abstract class AbstractBodyMod : MonoBehaviour
{
    //A reference to the owning gameobject's character controller
    public PlayerManager owner;
    public BodyModState macroState = BodyModState.INACTIVE;
    public BodyModType bodyModType;
    [System.NonSerialized]
    public ArmSide armSide;
    //used to provide finer control within the macro state
    //  for example, ACTIVE might have mini-states that this can keep track of
    public int microState = 0;
    //In seconds, how much time must pass before the player loses energy
    public float timePerTick = 1f;
    //related timer
    protected float elapsedTime = 0f;
    //Quantity of energy lost
    public int energyCostPerTick = 2;

    protected Animator animator;
    protected bool startingUp;
    protected bool tickDelay;

    public string name;

    //All of the above energy-related stuff should just be replaced with an object that decides when the player
    //  should lose energy, and then the body mod simply calls EnergyComponent.loseEnergy() every frame or something

    //what exactly these do will depend on the body mod
    //will be called every frame that the button is pressed
    //  body mod will affect properties of its owner
    public abstract void EnableBodyMod();
    public abstract void DisableBodyMod();
    public abstract void EquipBodyMod();
    public abstract void UnequipBodyMod();

    protected void GotoState(BodyModState state){
        macroState = state;
    }

    public void Awake()
    {
        animator = owner.animator;
    }

    protected IEnumerator StartUpDelay()
    {
        startingUp = true;
        yield return new WaitForSeconds(0.1f);
        startingUp = false;
    }

    protected IEnumerator DecreaseHealthAfterTick()
    {
        tickDelay = true;
        yield return new WaitForSeconds(timePerTick);
        owner.DecreaseHealth(energyCostPerTick, false);
        tickDelay = false;
    }

}
