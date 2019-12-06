using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
    public Animator eyesAnimator;

    bool enemyInRange, enemyWentOutOfRange;

    private void Start()
    {
        enemyInRange = false;
    }
    // Purpose of class: narrow eyes if enemies are in range
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12 || collision.gameObject.tag == "rangedEnemy")
        {
            if (!enemyInRange) StartCoroutine(EnemyInRange());
        }
    }

    IEnumerator EnemyInRange()
    {
        enemyInRange = true;
        //enable eyes
        eyesAnimator.SetBool("enemyInRange", true);

        yield return new WaitForSeconds(1);
        StartCoroutine(EnemyOutOfRangeCheck());
        enemyInRange = false;
    }

    IEnumerator EnemyOutOfRangeCheck()
    {
        yield return new WaitForSeconds(1.5f);
        if (!enemyInRange) eyesAnimator.SetBool("enemyInRange", false);
    }
}
