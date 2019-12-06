using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    // Start is called before the first frame update

    public float picDelay, teamDelay;
    public CanvasGroup picGroup, teamGroup;

    void Start()
    {
        StartCoroutine(CreditsSequence());
    }


    IEnumerator CreditsSequence()
    {
        yield return new WaitForSeconds(picDelay);

        StartCoroutine(FadeCanvasGroup(picGroup, teamGroup.alpha, 1, 1f));

        yield return new WaitForSeconds(teamDelay);

        StartCoroutine(FadeCanvasGroup(teamGroup, teamGroup.alpha, 1, 1f));

        /*
        yield return new WaitForSeconds(nextDelay);

        //do something
        */
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 1)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForFixedUpdate();
        }
    }
}
