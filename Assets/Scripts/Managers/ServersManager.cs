using UnityEngine;
using System.Collections;

//main Manager. Contains all servers: GameManager, inputManager and spawnerManager.
public class ServersManager{

    //Statics
    private static ServersManager _instance = null;

    private GameObject _servers;

    //Final enemies dead
    public int finalScore;


    //------------------------------------------------------------------------


    private ServersManager()
    {
        createServers();
    }

    //Function that create the servers
    void createServers()
    {
        //If exist the Servers
        _servers = GameObject.Find("Servers");
        //else
        if (_servers == null)
        {
            _servers = new GameObject("Servers");
        }

        //InputMgr
        AddServer<InputManager>();

        //GameManager
        AddServer<GameManager>();

        //SpawnerManager
        AddServer<SpawnerManager>();

        finalScore = 0;
    }

    //Funcion called for destroying this instance.
    public static void gameOver()
    {
        _instance = null;
    }

    public static ServersManager getSingleton()
    {
        if (_instance == null) _instance = new ServersManager();

        return _instance;
    }

    //Function for returning the indicated server 
    public T getServer<T>() where T : Component
    {
        T comp = null;
        if (_servers != null)
        {
            comp = _servers.GetComponent<T>();
        }

        return comp;
        
    }

    //Function for adding the indicated server
    public T AddServer<T>() where T : Component
    {
        if (_servers.GetComponent<T>() == null)
        {
            _servers.AddComponent<T>();
        }

        return _servers.GetComponent<T>();
    }

    
}
