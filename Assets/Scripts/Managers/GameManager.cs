using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Component associated by the gameObject "Servers", created by the serversManager.
public class GameManager : MonoBehaviour{


    //Selected turret in the HUD
    private string _turretSelected;
    public string turretSelected
    {
        get
        {
            if (_turretSelected == null)
                return "RedTurret"; //Defect is selected the red turret
            return _turretSelected;
        }
        set { _turretSelected = value; }
    }

    //maximum red turrets
    [SerializeField]
    private int numRedTurretsMax = 3;

    //maxium blue turrets
    [SerializeField]
    private int numBlueTurretsMax = 3;

    //Counter of the enemies dead
    private int enemiesDead;
    public int deadEnemies
    {
        get { return enemiesDead; }
    }

    //enemies alive
    private List<GameObject> enemies;
    public List<GameObject> listEnemies
    {
        get { return enemies; }
    }

    //maximun crystals
    [SerializeField]
    private int numCrystalsInitial = 2;

    //number of enemies at first
    [SerializeField]
    private int numEnemiesInitial = 5;
    private int numEnemiesMax;
    //Dimensions
    private int _wide;
    private int _width;

    public int wide
    {
        get { return _wide; }
    }

    public int width
    {
        get { return _width; }
    }

    //It is a matrix that stores if every tile is being occupied by other element, such as turrets, enemies or crystal. It will be
    //used when enemies are walking around the scenario and calculate their paths.
    private int[][] _OccupiedTiles;

    public int[][] OccupiedTiles
    {
        get { return _OccupiedTiles; }
    }

    //game prefabs. <type prefab, gameobject>. It is used for not using the function GameObject.Find
    private Dictionary<string, GameObject> _dictionaryPrefabs;

    public GameObject[] prefabsGame;

    //It is used for identifying the enemies's target
    public List<Vector3> positionCrystals;

    //spawnEnemy speed. Every 1 seconds a enemy is spawned
    public float timeBetweenSpawnEnemy = 1.0f;

    


    //-------------------------------------------------------------------------



    //enum that defines the differents gameobjects that can be over tiles
    private enum entity {EMPTY, TURRET, ENEMY, CRYSTAL};

	// Use this for initialization
    void Start()
    {
        //initializing local variables
        _dictionaryPrefabs = new Dictionary<string, GameObject>();
        positionCrystals = new List<Vector3>();
        enemies = new List<GameObject>();

        enemiesDead = 0;
        numEnemiesMax = numEnemiesInitial;

        //Debug.Log("GameManager Start");
        _wide = GameObject.FindGameObjectWithTag("Scenario").GetComponent<GenerateScenario>().wide;
        _width = GameObject.FindGameObjectWithTag("Scenario").GetComponent<GenerateScenario>().width;

        inicitialiseMatrix();

        initialisePrefabsGame();

        //We register the functions in the event "clickLeftButton" and "clickRightButton"
        ServersManager.getSingleton().getServer<InputManager>().RegisterClickLeftEvent(createTurret);
        ServersManager.getSingleton().getServer<InputManager>().RegisterClickRightEvent(destroyTurret);

        //HUD, turrets images with their counter
        _dictionaryPrefabs["TextRedTurret"].GetComponent<UnityEngine.UI.Text>().text = "X" + numRedTurretsMax;
        _dictionaryPrefabs["TextBlueTurret"].GetComponent<UnityEngine.UI.Text>().text = "X" + numBlueTurretsMax;

        //we create the crystals
        createCrystals();

        //coroutine for creating enemies
        StartCoroutine("createEnemy");

        //coroutine for adding one enemy more every 5 seconds.
        StartCoroutine("oneMoreEnemy");
	}

    void Update()
    {

    }

    IEnumerator oneMoreEnemy()
    {
        while (true)
        {
            ++numEnemiesMax;
            yield return new WaitForSeconds(5.0f);
        }
    }

    //function that creates enemies over time
    IEnumerator createEnemy()
    {
        System.Random rnd = new System.Random();
        int numberRandom = 0; 
        string typeEnemy = "";
        GameObject newEnemy = null;

        while (true)
        {
            //if there are less enemies than the max
            if (enemies.Count < numEnemiesMax)
            {
                //Choose the type of enemy
                numberRandom = rnd.Next(3);

                switch (numberRandom)
                {
                    case 0:
                        typeEnemy = "enemy_normal";
                        break;
                    case 1:
                        typeEnemy = "enemy_Swift";
                        break;
                    case 2:
                        typeEnemy = "enemy_Armoured";
                        break;
                }

                //Choose a random position
                numberRandom = rnd.Next(wide-1);

                //We create the enemy
                newEnemy = ServersManager.getSingleton().getServer<SpawnerManager>().createGameObject(_dictionaryPrefabs[typeEnemy],
                    new Vector3(0, 0, numberRandom*4), Quaternion.identity);

                //It is added in the list
                enemies.Add(newEnemy);
            }
            //Execute every timeBetweenSpawnEnemy seconds
            yield return new WaitForSeconds(timeBetweenSpawnEnemy); 
        }
    }

    //Function that destroy the enemy
    public void destroyEnemy(GameObject go)
    {
        //Deactivate the gameObject
        ServersManager.getSingleton().getServer<SpawnerManager>().destroyGameObject(go);
        //We remove this gameobject from the list
        enemies.Remove(go);

        //We add the deads enemies in the HUD.
        _dictionaryPrefabs["TextDeadEnemy"].GetComponent<UnityEngine.UI.Text>().text = "X" + ++enemiesDead;
    }

    //Function for initializing the matrix
    void inicitialiseMatrix()
    {
        //Initialise the matrix
        _OccupiedTiles = new int[width][];

        for (int widthCont = 0, wideCont = 0; widthCont < width; ++widthCont) //x
        {
            _OccupiedTiles[widthCont] = new int[wide];

            for (wideCont = 0; wideCont < wide; ++wideCont) //z
            {
                //We store 0 for indicating that there isn't anything in the tile [width][wide]
                _OccupiedTiles[widthCont][wideCont] = (int)entity.EMPTY;
            }
        }
    }

    //Function for initializing the dictionary related with the prefabs
    void initialisePrefabsGame()
    {
        foreach (GameObject go in prefabsGame)
        {
            _dictionaryPrefabs.Add(go.name, go);
        }
    }

    //Function for creating the turrets
    public void createTurret(Vector3 position)
    {
        //We obtain its tile position
        int x, z;
        getTilePosition(out x, out z, position.x, position.z);

        //We obtain the position in X maximum for putting the turret
        float posX = (float)(width - 1) * 0.3f * 4;

        //If this position is empty
        if (_OccupiedTiles[x][z] == (int)entity.EMPTY && posX <= position.x)
        {
            //We update the counter in the HUD
            if (turretSelected == "RedTurret" && numRedTurretsMax > 0)
            {
                _dictionaryPrefabs["TextRedTurret"].GetComponent<UnityEngine.UI.Text>().text = "X" + --numRedTurretsMax;
                
            }
            else if (turretSelected == "BlueTurret" && numBlueTurretsMax > 0)
            {
                _dictionaryPrefabs["TextBlueTurret"].GetComponent<UnityEngine.UI.Text>().text = "X" + --numBlueTurretsMax;
            }
            else
            {
                return;
            }

            //We create the turret in this tile position. 4 is the size.
            ServersManager.getSingleton().getServer<SpawnerManager>().createGameObject(_dictionaryPrefabs[turretSelected],
                new Vector3(x * 4, transform.position.y, z * 4), _dictionaryPrefabs[turretSelected].transform.rotation);

            //We update the matrix
            OccupiedTiles[x][z] = (int)entity.TURRET;
        }
    }

    //Function for destroying the turret
    public void destroyTurret(GameObject go)
    {
        //Deactivate the gameobject
        ServersManager.getSingleton().getServer<SpawnerManager>().destroyGameObject(go);

        //We update the matrix
        int x, z;
        getTilePosition(out x, out z, go.transform.position.x, go.transform.position.z);
        OccupiedTiles[x][z] = (int)entity.EMPTY;

        //And update the counter in the HUD
        string name = go.name;

        if (go.name.IndexOf("@") >= 0)
        {
            name = go.name.Split('@')[0];

            if (name == "RedTurret")
            {
                _dictionaryPrefabs["TextRedTurret"].GetComponent<UnityEngine.UI.Text>().text = "X" + ++numRedTurretsMax;
            }
            else if (name == "BlueTurret")
            {
                _dictionaryPrefabs["TextBlueTurret"].GetComponent<UnityEngine.UI.Text>().text = "X" + ++numBlueTurretsMax;
            }
        }

            
    }

    //Function for creating the crystals
    public void createCrystals()
    {
        System.Random random = new System.Random();
        int tileSelected = 0;

        for (int i = 0; i < numCrystalsInitial; ++i)
        {
            do
            {
                //Choose a random position
                tileSelected = random.Next(wide);
            } while (_OccupiedTiles[width-1][tileSelected] != (int)entity.EMPTY);

            //We create the crystal
            ServersManager.getSingleton().getServer<SpawnerManager>().createGameObject(_dictionaryPrefabs["Crystal"],
                new Vector3(4*(width - 1), _dictionaryPrefabs["Crystal"].transform.position.y, tileSelected * 4), Quaternion.identity);
            
            //We update the matrix
            _OccupiedTiles[width - 1][tileSelected] = (int)entity.CRYSTAL;

            //And add this crystal to the list
            positionCrystals.Add(new Vector3((width - 1), 0, tileSelected));
        }
    }

    //Function that indicates if this position is occupied by another object
    public bool isTileOccupied(Vector3 position)
    {
        int x, z;
        getTilePosition(out x, out z, position.x, position.z);

        return _OccupiedTiles[x][z] != (int)entity.EMPTY && _OccupiedTiles[x][z] != (int)entity.CRYSTAL;
    }

    //Function for obtaining the tile position
    public void getTilePosition(out int tilePositionX, out int tilePositionZ, float worldPositionX, float worldPositionZ)
    {
        //*0.25 = /4
        tilePositionX = (int)Mathf.Round(worldPositionX*0.25f);
        tilePositionZ = (int)Mathf.Round(worldPositionZ * 0.25f);
    }

    //Function that updates the matrix in the indicated position
    public void updateOccupiedTiles(int width, int wide, int type)
    {
        OccupiedTiles[width][wide] = type;
    }

    
    
}
