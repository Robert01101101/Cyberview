using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationHelper : MonoBehaviour
{
    public GameObject box, shovel;
    public FinalBoss finalBoss;

    public void GrabBox(GameObject boxToGrab)
    {
        box = boxToGrab;
        box.tag = "Untagged";
    }

    public void ShovelDown()
    {
        if (box != null) box.transform.SetParent(shovel.transform);
    }

    public void Crunch()
    {
        if (box != null)
        {
            Destroy(box);
            finalBoss.Damage();
        }
    }
}
