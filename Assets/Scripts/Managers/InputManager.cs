using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//Component associated by the gameObject "Servers", created by the serversManager.
public class InputManager : MonoBehaviour
{
    //Events related with the clicks
    public delegate void clickLeftEvent(Vector3 pointClick);
    public delegate void clickRightEvent(GameObject go);

    private clickLeftEvent clickLeftPressed = null;
    private clickRightEvent clickRightPressed = null;

    //------------------------------------------------------------------------



    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        onClick();
    }

    void throwRay(int buttonDown)
    {
        //We launch a ray from the camera position indicated to mouse position.
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

        //If there is collision
		if (Physics.Raycast(ray,out hit)) 
		{
            //left click is used for putting turrets
            if (buttonDown == 0 && clickLeftPressed != null && hit.collider.gameObject.tag == "TILE")
            {
                clickLeftPressed(hit.point);
            }
            //right click is used for removing turrets
            else if (buttonDown == 1 && clickRightPressed != null && hit.collider.gameObject.tag == "Turret")
            {
                
                clickRightPressed(hit.collider.gameObject);
            }
        }

    }

    void onClick()
    {
        //If we push left button
        if (Input.GetMouseButtonDown(0))
        {
            throwRay(0);
        }

        if (Input.GetMouseButtonDown(1))
        {         
            throwRay(1);
        }
    }


    //Functions for registering the events
    public void RegisterClickLeftEvent(clickLeftEvent begin)
    {
        if (begin != null)
        {
            clickLeftPressed += begin;
        }
    }

    public void RegisterClickRightEvent(clickRightEvent begin)
    {
        if (begin != null)
        {
            clickRightPressed += begin;
        }
    }


    
}
