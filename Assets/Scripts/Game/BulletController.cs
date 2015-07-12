using UnityEngine;
using System.Collections;

//Component associated with the "Bullet". It is used for move the bullet from its position to its target.
public class BulletController : MonoBehaviour {

    //bullet speed
    public float speed = 10.0f;

    //damage that is applied when the bullet collides with its target
    public float damage = 20.0f;

    public Vector3 directionTarget
    {
        set { _directionTarget = value; }
    }
    
//-------------------------------------------------------------------------


	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        //We move the bullet to its target
        transform.position += _directionTarget * speed * Time.deltaTime;
	}

    void OnTriggerEnter(Collider other)
    {
        //if the bullet collides with a enemy
        if (other.gameObject.tag == "Enemy")
        {
            //The bullet is destroyed
            ServersManager.getSingleton().getServer<SpawnerManager>().destroyGameObject(this.gameObject);
            //and we apply damage to enemy
            other.gameObject.GetComponent<LifeController>().applyDamage(damage);
        }
    }

    //Direction from bulletPosition to enemyPosition
    private Vector3 _directionTarget;

}
