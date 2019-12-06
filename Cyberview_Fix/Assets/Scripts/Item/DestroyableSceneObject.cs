using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableSceneObject : AbstractLvlItem
{
    public Sprite solid, translucent;
    private SpriteRenderer spriteRenderer;
    private bool blinking = false;
    public int ticsToBreak = 2;
    private float blinkLength = .7f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = solid;
    }

    public void Hit()
    {
        ticsToBreak--;
        if (ticsToBreak == 0)
        {
            //spawn random reward
            GameObject.Find("LevelManager").GetComponent<LvlManager>().SpawnRandomReward(gameObject.transform.position);

            //save state
            PlayerPrefs.SetInt(objectID, 1);

            Destroy(gameObject);
        }
        else if (!blinking)
        {
            StartCoroutine(Blink());
        }
    }

    IEnumerator Blink()
    {
        blinking = true;

        //Blink
        spriteRenderer.sprite = translucent;
        yield return new WaitForSeconds(blinkLength / 2);
        spriteRenderer.sprite = solid;
        yield return new WaitForSeconds(blinkLength / 2);

        blinking = false;
    }
}
