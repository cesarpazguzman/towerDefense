using UnityEngine;
using System.Collections;

//Component associated with the tile. Its used for giving feedback
public class TileController : MonoBehaviour {

    //Colour initial of the tile
    private Color colorInitial;
    private GameManager GameMgr;

    //------------------------------------------------------------------------


    void Awake()
    {
        GameMgr = ServersManager.getSingleton().getServer<GameManager>();
        
    }

	// Use this for initialization
	void Start () 
    {
        colorInitial = this.GetComponent<MeshRenderer>().material.GetColor("_Color");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Function called when the mouse enter in this tile
    void OnMouseEnter()
    {
        //We obtain the width
        int size = GameObject.FindGameObjectWithTag("Scenario").GetComponent<GenerateScenario>().width;

        //We obtain the position X where is permitted put tiles
        float posX = (float)(size - 1) * 0.3f * 4;

        
        Color colour = Color.red;
        if (posX <= transform.position.x && !GameMgr.isTileOccupied(transform.position))
        {
            //if this tile is empty and is in a permitted position
            colour = Color.green;
        }

        this.GetComponent<MeshRenderer>().material.SetColor("_Color", colour);
    }

    void OnMouseExit()
    {
        this.GetComponent<MeshRenderer>().material.SetColor("_Color", colorInitial);
    }
}


