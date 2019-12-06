using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc241 : MonoBehaviour
{
    public bool walk, escapeFactory;
    public GameObject crate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (walk && !escapeFactory)
        {
            gameObject.transform.position += new Vector3(.17f, 0, 0);
            GetComponent<Animator>().SetBool("run", true);
        } else if (walk && escapeFactory)
        {
            gameObject.transform.position += new Vector3(.17f, 0, 0);
            GetComponent<Animator>().SetBool("run", true);
            GetComponent<Animator>().SetBool("grab", false);
            GetComponent<Animator>().SetBool("raiseArmR", false);
            if (crate != null) crate.SetActive(false);
        }
    }
}
