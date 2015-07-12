using UnityEngine;
using System.Collections;

//Components that is used for destroying the gameobject after several seconds. Bullets and smokeParticle have this component
public class TimeToLive : MonoBehaviour {

    [SerializeField]
    private float timeLiveSeconds;

    private float currentTime;

    //------------------------------------------------------------------------


	// Use this for initialization
	void Start () {
	
	}

    void OnEnable()
    {
        currentTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;
        if (currentTime >= timeLiveSeconds)
        {
            ServersManager.getSingleton().getServer<SpawnerManager>().destroyGameObject(this.gameObject);
        }
	}

    
}
