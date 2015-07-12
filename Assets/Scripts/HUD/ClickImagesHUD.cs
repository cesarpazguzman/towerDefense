using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//Component associated with the images "ImageRedTurret" and "ImageBlueTurret". It is used for giving feedback and selecting the turret
public class ClickImagesHUD : MonoBehaviour {

    //Local variable used for save its initial colour
    private Color initialColour;

    //-------------------------------------------------------------------------

	// Use this for initialization
	void Start () 
    {
        //Save its colour
        initialColour = this.GetComponent<Image>().color;
	}
	
	// Update is called once per frame
	void Update () 
    {
    }

    //Events-----------
    
    //Event called when the mouse is over the image "imageRedTurret"
    public void turnRed()
    {
        this.GetComponent<Image>().color = Color.red;
    }

    //Event called when the mouse is over the image "imageBlueTurret"
    public void turnBlue()
    {
        this.GetComponent<Image>().color = Color.blue;
    }

    //Event called when the mouse exits the image
    public void turnNormal()
    {
        this.GetComponent<Image>().color = initialColour;
    }

    //Event called when the mouse is pressed over the images. it is used for change the turret 
    public void OnClick()
    {
        if (this.gameObject.name == "ImageRedTurret")
        {
            ServersManager.getSingleton().getServer<GameManager>().turretSelected = "RedTurret";
        }
        else if (this.gameObject.name == "ImageBlueTurret")
        {
            ServersManager.getSingleton().getServer<GameManager>().turretSelected = "BlueTurret";
        }
        
    }

    
}
