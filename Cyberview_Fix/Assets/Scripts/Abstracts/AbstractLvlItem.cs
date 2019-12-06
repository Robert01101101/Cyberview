using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractLvlItem : MonoBehaviour
{
    // This class assigns a unique object ID (Lvl name + location of item) and destroys the object if it has already been collected / used
    // -> Subclasses need to register that they have been collected / used by setting: PlayerPrefs.SetInt(objectID, 1);

    [System.NonSerialized]
    public string objectID;
    private void Awake()
    {
        objectID = gameObject.scene.name + ", x=" + gameObject.transform.position.x + ", y=" + gameObject.transform.position.y;
    }

    public void DisableItemIfCollected()
    {
        if (PlayerPrefs.HasKey(objectID)) gameObject.SetActive(false);
    }

    public virtual void ReenableItem()
    {
        gameObject.SetActive(true);
        if (PlayerPrefs.HasKey(objectID)) PlayerPrefs.DeleteKey(objectID);
    }
}
