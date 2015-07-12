using UnityEngine;
using System.Collections;


//Component abstract. It will be inherited by the red and blue turrets
public abstract class TurretScript : MonoBehaviour {
  

    protected GameManager gameMgr;

    //------------------------------------------------------------------------



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {}

    //Function for looking for a target
    protected GameObject findTarget()
    {
        float distanceNearest = 1000 * 1000;
        float distanceCalculate = 0;
        GameObject nearest = null;

        if (gameMgr.listEnemies == null) return null;

        //Every enemy
        foreach (GameObject go in gameMgr.listEnemies)
        {
            //We calculate the distance. squareDistance.
            distanceCalculate = calculateDistance(transform.position, go.transform.position);
            if (distanceCalculate < distanceNearest)
            {
                distanceNearest = distanceCalculate;
                nearest = go;
            }
        }

        //We obtain the nearest
        return nearest;
    }

    //Function for calculating the distance between two points.
    protected float calculateDistance(Vector3 source, Vector3 target)
    {
        return (source.x - target.x) * (source.x - target.x) + (source.y - target.y) * (source.y - target.y);
    }

    //This function will be implemented by its children
    protected abstract IEnumerator shootEnemy();
   

}
