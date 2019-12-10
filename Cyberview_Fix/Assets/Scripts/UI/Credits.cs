using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    // Start is called before the first frame update
    public CanvasGroup logoCG;
    public Animator roll;
    public GameObject quitBtn;

    void Start()
    {
        StartCoroutine(CreditsSequence());
        PlayerPrefs.DeleteAll();
        GameObject.Find("_HUD").SetActive(false);
        GameObject.Find("Sound").SetActive(false);
    }


    IEnumerator CreditsSequence()
    {
        yield return new WaitForSeconds(.3f);
        logoCG.gameObject.SetActive(true);
        StartCoroutine(FadeCanvasGroup(logoCG, logoCG.alpha, 1, 3.5f));
        //StartCoroutine(ShrinkLogo(logo, 4));

        yield return new WaitForSeconds(6.3f);
        roll.enabled = true;

        yield return new WaitForSeconds(40.5f);
        quitBtn.SetActive(true);
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

    public void QuitBtn()
    {
        Application.Quit();
        Debug.Log("Credits -> Quit");
    }
}
