using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource footstep1, footstep2, pickup, jump, boulder, door, hit, laserHit, bulletHit, music;
    public PlayerManager owner;
    public AudioClip bossMusic;

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

    public void EnemyHit()
    {
        hit.Stop();
        hit.Play();
    }

    public void LaserBulletHit()
    {
        laserHit.Stop();
        laserHit.Play();
    }

    public void BulletHit()
    {
        bulletHit.Stop();
        bulletHit.Play();
    }

    public void BossMusic()
    {
        music.clip = bossMusic;
        music.volume = .5f;
        music.Stop();
        music.Play();
    }


}
