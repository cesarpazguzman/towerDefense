using UnityEngine;
using System.Collections;

//Component associated with the main camera. It defines its behaviour when the player uses the controls for moving the camera
public class CameraController : MonoBehaviour {


    //zoom speed
    public float zoomSpeed = 40f;

    //min field of view when we do zoom in
    public float minZoomFOV = 5f;

    //local variable used for keep the position Y
    private float _posY;

    //-------------------------------------------------------------------------

	// Use this for initialization
	void Start () 
    {
        //Save the position.y intial
        _posY = GetComponent<Camera>().transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {

        rotation();

        //Zoom
        zoom();

        //Move
        panning();

	}

    void panning()
    {
        //angle = 30. In the horizontal movements, we keep the position in the axis Y
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.right * -30 * Time.deltaTime, Space.Self);
            transform.position = new Vector3(transform.position.x, _posY, transform.position.z);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * 30 * Time.deltaTime, Space.Self);
            transform.position = new Vector3(transform.position.x, _posY, transform.position.z);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector3.up * -30 * Time.deltaTime, Space.Self);
            _posY = GetComponent<Camera>().transform.position.y;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(Vector3.up * 30 * Time.deltaTime, Space.Self);
            _posY = GetComponent<Camera>().transform.position.y;
        }
    }

    void zoom()
    {
        if (Input.GetKey(KeyCode.W))
        {
            zoomIn();
        }
        if (Input.GetKey(KeyCode.S))
        {
            zoomOut();
        }
    }

    void zoomIn()
    {
        //We modified the fieldOfView
        Camera.main.fieldOfView -= zoomSpeed * Time.deltaTime;

        if (Camera.main.fieldOfView < minZoomFOV)
        {
            Camera.main.fieldOfView = minZoomFOV;
        }
    }

    void zoomOut()
    {
        //We modified the fieldOfView
        Camera.main.fieldOfView += zoomSpeed * Time.deltaTime;
    }

    void rotation()
    {
        //Only is possible the rotation if we do right click.
        if (Input.GetMouseButton(1))
        {
            //We rotate around the current position, 5 degrees every time
            transform.RotateAround(transform.position, transform.right, -Input.GetAxis("Mouse Y") * 5);
            transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X") * 5);
        }
    }
}

