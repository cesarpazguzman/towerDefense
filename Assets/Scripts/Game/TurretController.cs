using UnityEngine;
using System.Collections;

//Component associated with the blue turret.
public class TurretController : TurretScript
{
    //bullet to shoot
    [SerializeField]
    private GameObject bullet;

    public float rate = 1.0f;

    //------------------------------------------------------------------------


    void Awake()
    {
        gameMgr = ServersManager.getSingleton().getServer<GameManager>();
    }

	// Use this for initialization
	void Start () {
        
	}

    void OnEnable()
    {
        //We put this here, because we can remove the turret
        StartCoroutine("shootEnemy");
    }

	// Update is called once per frame
	void Update () 
    {
        
	}

    protected override IEnumerator shootEnemy()
    {
        
        Vector3 dirTarget = Vector3.zero;
        float angle = 0;
        GameObject target = null;

        while (true)
        {
            //We look for a target
            target = findTarget();

            //if it has a target and the bullet exists, 
            if (target != null && bullet != null)
            {
                //We create a new bullet
                GameObject newBullet = ServersManager.getSingleton().getServer<SpawnerManager>().createGameObject(bullet, transform.position+new Vector3(0,2,0), bullet.transform.rotation);

                //We obtain its direction
                dirTarget = (target.transform.position + new Vector3(0, 2, 0)) - newBullet.transform.position;
                dirTarget.Normalize();

                //its angle for seeing to its target
                angle = Vector3.Angle(newBullet.transform.right, dirTarget);

                //If its target is behind, then we change the sign of the angle
                if (target.transform.position.x > transform.position.x)
                {
                    angle *= -1;
                }

                //We rotate the bullet with the calculated angle
                newBullet.transform.RotateAround(newBullet.transform.position, Vector3.up, angle);

                //We indicate to bullet its target
                newBullet.GetComponent<BulletController>().directionTarget = dirTarget;
                
            }
            yield return new WaitForSeconds(rate); //Shoot every rate seconds
        }
    }

    
}
 