using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource footstep1, footstep2, pickup, jump, boulder, door;
    public PlayerManager owner;

    public void SoundFootStep1()
    {
        footstep1.Stop();
        footstep1.Play();
    }

    public void SoundFootStep2()
    {
        footstep2.Stop();
        footstep2.Play();
    }

    public void SoundPickup()
    {
        pickup.Stop();
        pickup.Play();
    }

    public void SoundJump()
    {
        footstep1.Stop();
        footstep2.Stop();
        jump.Stop();
        jump.Play();
    }
    public void SoundBoulder()
    {
        boulder.Stop();
        boulder.Play();
    }
    public void SoundDoor()
    {
        door.Stop();
        door.Play();
    }

    public void EyesHurt()
    {
        GetComponent<Animator>().SetBool("hurt", false);
    }

    public void ArmRaised()
    {
        owner.bm_Gun.ArmRaised();
    }

    public void ArmLowered()
    {
        owner.bm_Gun.ArmLowered();
    }


}
