using UnityEngine;
using System.Collections;

//Component asociated with text "deads" in the menu scene. It is used for showing the final result
public class FinalScore : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        GetComponent<UnityEngine.UI.Text>().text = "" + ServersManager.getSingleton().finalScore;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
