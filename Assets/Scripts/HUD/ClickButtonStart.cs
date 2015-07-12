using UnityEngine;
using System.Collections;

//Component associated with the Buttons corresponding at "Play" and "GameOver"
public class ClickButtonStart : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    //Event called when we push the button play game or game over
    public void startGame()
    {
        //instance of ServersManager = null, because in a new game, it's neccesary initialize the managers again
        ServersManager.gameOver();

        //We load leve game
        Application.LoadLevel("game");

    }

}
