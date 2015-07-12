using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Component associated with the Scenario
public class GenerateScenario : MonoBehaviour {

    [SerializeField]
    private GameObject tile;
    [SerializeField]
    private int _wide; //number of tiles: wide and width (squared scenario)

    [SerializeField]
    private int _width; //number of tiles: wide and width (squared scenario)
    public int width
    {
        get { return _width; }
    }

    public int wide
    {
        get { return _wide; }
    }

    //-------------------------------------------------------------------------


	// Use this for initialization
	void Start () 
    {
	    //We create the scenario
        createScenario();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void createScenario()
    {
        for (int widthCont = 0, wideCont = 0; widthCont < width; ++widthCont) //x
        {
            for (wideCont = 0; wideCont < wide; ++wideCont) //z
            {
                //We create every tile. 4 is the size
                GameObject obj = GameObject.Instantiate(tile, new Vector3(widthCont * 4, 0, wideCont * 4), Quaternion.identity) as GameObject;
                obj.transform.parent = this.transform;
            }
        }
    }
}
