using UnityEngine;
using System.Collections;

//Component associated with the red turret. 
public class LserBeamController : TurretScript
{
    //Local variable that indicates their target
    private Vector3 _targetPosition;
    public Vector3 targetPosition
    {
        set { _targetPosition = value; }
    }

    //damage laser
    public float damage;


    //------------------------------------------------------------------------



    void Awake()
    {
        gameMgr = ServersManager.getSingleton().getServer<GameManager>();
    }

	// Use this for initialization
    void Start()
    {
        //We create the component lineRenderer
        LineRenderer lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetColors(Color.blue, Color.blue);
        lineRenderer.SetWidth(1.0f,1.0f);
        lineRenderer.SetVertexCount(2);

        _targetPosition = transform.position;

	}

    void OnEnable()
    {
       //We put this here, because we can remove the turret
       StartCoroutine("shootEnemy");
    }

	// Update is called once per frame
	void Update () {
	
	}


    void FixedUpdate()
    {
        //We update the target position
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.SetPosition(1, _targetPosition + new Vector3(0, 5, 0));
        lineRenderer.SetPosition(0, transform.position + new Vector3(0, 5, 0));
    }

    //Function that creates the laser
    protected override IEnumerator shootEnemy()
    {
        GameObject target = null;

        while (true)
        {
            //We look for a target
            target = findTarget();

            _targetPosition = transform.position;

            //If we find a target
            if (target != null)
            {
                //update the target Position
                _targetPosition = target.transform.position;
                //we apply damage every 0.2 seconds. (5 times every second)
                target.GetComponent<LifeController>().applyDamage(damage);
            }
        
            yield return new WaitForSeconds(0.2f); //Cambia de objetivo 5 veces por segundo
        }
    }

   
}
