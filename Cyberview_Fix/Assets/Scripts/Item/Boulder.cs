using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : AbstractLvlItem
{
    SpriteRenderer spriteRenderer;
    public Sprite solid, translucent;
    private int ticsToBreak = 3;
    private float ticLength = .5f;
    private bool delaying = false;

    void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = solid;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Drill" && !delaying)
        {
            ticsToBreak--;
            delaying = true;

            if (ticsToBreak < 0)
            {
                DestroyBoulder();
            } else
            {
                StartCoroutine(Dig());
            }
        }
    }

    IEnumerator Dig()
    {
        Debug.Log("Boulder -> Dig");

        //Blink
        spriteRenderer.sprite = translucent;
        yield return new WaitForSeconds(ticLength / 2);
        spriteRenderer.sprite = solid;
        yield return new WaitForSeconds(ticLength / 2);

        delaying = false;
    }

    void DestroyBoulder()
    {
        Debug.Log("Boulder -> DestroyBoulder");
        PlayerManager player = GameObject.Find("_Player").GetComponent<PlayerManager>();
        player.RemoveInteractable(gameObject);
        //player.GetPlayerSound().SoundBoulder();
        player.bm_Drill.BoulderDestroyed();

        //save state
        PlayerPrefs.SetInt(objectID, 1);

        Destroy(gameObject);
    }
}
