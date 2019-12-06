using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace cutScenes{
	public class Cutscene1_dialogue : MonoBehaviour
	{
		Text text;
	    // Start is called before the first frame update
	    void Awake()
	    {
	     text = GetComponent <Text>();

	    }

	    // Update is called once per frame
	    void Update()
	    {
	        StartCoroutine(MyCoRoutine());
	    }

		IEnumerator MyCoRoutine()
		{
		     text.text = "Let's see here.";
		     yield return new WaitForSeconds(3);
	
		     text.text = "Wait.";
		   	 yield return new WaitForSeconds(3);
		   
		     text.text = "Your inhibitor is broken!";
		      yield return new WaitForSeconds(3);
		     
		     text.text = "Oh my god!";
		      yield return new WaitForSeconds(2);

		     text.text = "Put down that drill!";
		     yield break;
		}
	}
}