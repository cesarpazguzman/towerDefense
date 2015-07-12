using UnityEngine;
using System.Collections;

//Component associated with the enemies. It controls their life.
public class LifeController : MonoBehaviour {

    public GameObject healthBar;

    //enemy health
    [SerializeField]
    private float health;

    //Local variable for the current health
    private float currentHealth;

    //Gameobject created when this enemy deads.
    public GameObject deadSmoke;

    //------------------------------------------------------------------------


	// Use this for initialization
	void Start () {
        currentHealth = health;
        //Initialize the life Bar
        healthBar.transform.localScale = new Vector3(currentHealth / health, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Function called when the laser or the bullet collide with this enemy
    public void applyDamage(float damage)
    {
        //We substract the current health
        currentHealth -= damage;

        healthBar.transform.localScale = new Vector3(currentHealth / health, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        
        //If it is less than 0
        if (currentHealth <= 0)
        {
            //Deactivate the enemy and create the smoke
            currentHealth = health;
            healthBar.transform.localScale = new Vector3(currentHealth / health, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
            ServersManager.getSingleton().getServer<GameManager>().destroyEnemy(this.gameObject);
            ServersManager.getSingleton().getServer<SpawnerManager>().createGameObject(deadSmoke, this.transform.position + new Vector3(0,2,0), Quaternion.identity);
            
            
        }
    }

    
}
