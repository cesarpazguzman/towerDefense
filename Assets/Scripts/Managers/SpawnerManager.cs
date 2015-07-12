using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Component associated by the gameObject "Servers", created by the serversManager.
public class SpawnerManager : MonoBehaviour {

    //Cache used for saving the deactivated gameobjects. It's a pool. When it's neccesary create a gameobject, 
    //firstly this gameobject is looked for in this pool.
    private Dictionary<string, List<GameObject>> _cache;
    private static int _currentID = 0;

    //Class that represents every prefab. 
    [System.Serializable]
    public class CacheData
    {
        public GameObject prefab;
        public int cacheSize; //number of deactivated prefabs
    }
    //Array of cacheData. Contains several prefabs that will be instanciate at first.
    public CacheData[] objectCache;


    //-------------------------------------------------------------------------


	// Use this for initialization
	void Start () 
    {
        _cache = new Dictionary<string, List<GameObject>>();
        InstanciateInitialObjects();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnDestroy()
    {
        //We clear the cache
        ClearCache();
    }

    //Functions for creating the gameobject
    public GameObject createGameObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject go = null;

        //If contains this prefab
        if (_cache.ContainsKey(prefab.name))
        {
            //We obtain the list of this prefab
            List<GameObject> list = _cache[prefab.name];
            int numElements = list.Count;

            //If there is this prefab
            if (numElements > 0)
            {
                //We extract it
                go = list[numElements - 1];
                list.RemoveAt(numElements - 1);
                //We set its new position and orientation
                go.transform.position = position;
                go.transform.rotation = rotation;
                //We active it
                go.SetActive(true);           
            }
        }

        //If this prefab isnt deactivated, we have to create it
        if(go == null)
        {
            go = GameObject.Instantiate(prefab, position, rotation) as GameObject;

            //New name.
            go.name = prefab.name + "@" + ++_currentID;

            //All objects in this scene are children of Game.
            go.transform.parent = GameObject.Find("Game").transform;
        }

        return go;
    }

    //Function for destroying or deactivating the gameobject
    public void destroyGameObject(GameObject go, bool destroy = false)
    {
        //if destroy is true, then we destroy it
        if (destroy)
        {
            GameObject.Destroy(go);
        }
        else
        {
            //Else, we deactivate it
            go.SetActive(false);
            string originalPrefabName = go.name;

            //obtain its name
            if (go.name.IndexOf("@") >= 0)
            {
                originalPrefabName = go.name.Split('@')[0];
            }
            
            //If cache contains this name
            if (_cache.ContainsKey(originalPrefabName))
            {
                //We add this prefab in the list
                _cache[originalPrefabName].Add(go);
            }
            else
            {
                //else we create a new list.
                List<GameObject> list = new List<GameObject>();
                list.Add(go);
                _cache.Add(originalPrefabName, list);
            }
        }
    }

    //Function that instanciate initial objects indicate in the array cacheData
    public void InstanciateInitialObjects()
    {
        foreach (CacheData obj in objectCache)
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < obj.cacheSize; ++i)
            {
                GameObject gameObj = GameObject.Instantiate(obj.prefab, Vector3.zero, Quaternion.identity) as GameObject;
                gameObj.name = obj.prefab.name + "@" + ++_currentID;
                gameObj.SetActive(false);
                //Add this gameobject in the list
                list.Add(gameObj);
                //its parent is Game
                gameObj.transform.parent = GameObject.Find("Game").transform;    
            }
            _cache.Add(obj.prefab.name, list);
        }
    }

    //Function that destroys  all deactivated objects
    public void ClearCache()
    {
        foreach (List<GameObject> obj in _cache.Values)
        {
            foreach (GameObject go in obj)
            {
                GameObject.Destroy(go);
            }
            obj.Clear();
        }
        _cache.Clear();
    }

   

    

}
