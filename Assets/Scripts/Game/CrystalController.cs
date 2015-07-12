using UnityEngine;
using System.Collections;

//Component associated with the crystals. 
public class CrystalController : MonoBehaviour {

    //enemies dead
    private int deads;

    //-------------------------------------------------------------------------


	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
        //The crystal rotates over time
        transform.Rotate(Vector3.up, 80 * Time.deltaTime);
	}

    void OnTriggerEnter(Collider other)
    {
        //If a enemy collides with this crystal, then the game finished. GAMEOVER
        if (other.tag == "Enemy")
        {
            //update the enemies dead
            deads = ServersManager.getSingleton().getServer<GameManager>().deadEnemies;

            //Load gameover
            Application.LoadLevel("menu");

            //Apply the final score
            ServersManager.getSingleton().finalScore = deads;

        }
    }

    


}
