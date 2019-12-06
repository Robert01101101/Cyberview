using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCbasicEnemy : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += new Vector3(.08f, 0, 0);
    }
}
