using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Component associated with the enemies. Execute the algorithm BFS. 
public class EnemyController : MonoBehaviour {

    //Speed enemy
    [SerializeField]
    private float speed;

    //Next node which the enemy has to go.
    private Vector3 targetPosition;

    //grafo that contains all passable nodes
    private Node[][] grafo;

    //Dimensions
    private int width;
    private int wide;

    //Local variable that saves the unexplored nodes, but visitted
    private Queue<Node> nodesToExplore;

    //path from the current node to the target node (node corresponding with the nearest crystal)
    private List<Node> path;

    //Class correspoding with a node
    public class Node
    {
        //Saves if has already been visited
        public bool state = false;
        //Saves the distance from the source node
        public int distance = 0;
        //position 
        public Vector3 position = Vector3.zero;
        //Its parent
        public Node previousNode = null;
    }


    //-------------------------------------------------------------------------



	// Use this for initialization
	void Start () {
        
        //Initialize the variables
        nodesToExplore = new Queue<Node>();

        width = ServersManager.getSingleton().getServer<GameManager>().width;
        wide = ServersManager.getSingleton().getServer<GameManager>().wide;

        initialiseGrafo();

        targetPosition = transform.position;

        path = new List<Node>();

        //Execute the algorithm BFS
        BFS();
	}
	
	// Update is called once per frame
	void Update () 
    {
        //If the enemy has a target
        if (targetPosition != Vector3.zero)
        {
            //We move the enemy
            Vector3 dir = targetPosition - transform.position;
            dir.Normalize();

            transform.position += dir * speed * Time.deltaTime;

            //We check if the enemy reaches the target
            targetReady();
        }
	}

    //Function that checks if the enemy reaches the target
    void targetReady()
    {
        //squareDistance
        float distance = (transform.position.x - targetPosition.x) * (transform.position.x - targetPosition.x) + 
            (transform.position.z - targetPosition.z)*(transform.position.z - targetPosition.z);

        if (distance < 2)
        {
            //if the enemy reaches the target, then we execute the algorithm BFS again (Because the other enemies are moving to and
            //y the player could change the turrets)
            BFS();
        }
    }

    //new grafo
    void initialiseGrafo()
    {

        grafo = new Node[width][];

        for (int widthCont = 0, wideCont = 0; widthCont < width; ++widthCont) //x
        {
            grafo[widthCont] = new Node[wide];
            for (wideCont = 0; wideCont < wide; ++wideCont) //x
            {
                grafo[widthCont][wideCont] = new Node();
            }
        }
    }

    //Clean all nodes
    void initialiseNodes()
    {
        for (int widthCont = 0, wideCont = 0; widthCont < width; ++widthCont) //x
        {
            for (wideCont = 0; wideCont < wide; ++wideCont) //x
            {
                grafo[widthCont][wideCont].state = false;
                grafo[widthCont][wideCont].distance = 0;
                grafo[widthCont][wideCont].previousNode = null;
                grafo[widthCont][wideCont].position = new Vector3(0, 0, 0);
            }
        }

        path.Clear();
    }

    //Algorithm BFS
    void BFS()
    {

        //initialize local variables
        Node nodeExtracted = null;

        List<Node> nodesAdyacentes = new List<Node>();

        initialiseNodes();

        //We obtain the tile position where is the enemy
        int x, z;
        ServersManager.getSingleton().getServer<GameManager>().getTilePosition(out x, out z, transform.position.x, transform.position.z);

        //Source node
        grafo[x][z].state = true;
        grafo[x][z].distance = 0;
        grafo[x][z].previousNode = null;
        grafo[x][z].position = new Vector3(x, transform.position.y, z);

        //clean the queue
        nodesToExplore.Clear();

        //We put this node in the queue
        nodesToExplore.Enqueue(grafo[x][z]);

        //While the queue saves nodes
        while (nodesToExplore.Count > 0)
        {
            nodesAdyacentes.Clear();

            //Extract the next node and we explore it
            nodeExtracted = nodesToExplore.Dequeue();

            //We obtain the passable nodes
            getNodesAdyacentes((int)nodeExtracted.position.x, (int)nodeExtracted.position.z, ref nodesAdyacentes);

            foreach (Node nodeAdyacente in nodesAdyacentes)
            {
                //Si this node hasnt been visited
                if (!nodeAdyacente.state)
                {
                    nodeAdyacente.state = true;
                    nodeAdyacente.distance = nodeExtracted.distance +1;
                    nodeAdyacente.previousNode = nodeExtracted;
                    nodesToExplore.Enqueue(nodeAdyacente);
                }
            }
        }

        //Finally, we generate the final path.
        generatePath();
    }

    //Generate the final path
    void generatePath()
    {
        int shortestDistance = 99999;
        Node nodeNearest = null;
        
        //We obtain the nearest crystal
        foreach (Vector3 pos in ServersManager.getSingleton().getServer<GameManager>().positionCrystals)
        {
            if (shortestDistance > grafo[(int)pos.x][(int)pos.z].distance && grafo[(int)pos.x][(int)pos.z].distance > 0)
            {
                shortestDistance = grafo[(int)pos.x][(int)pos.z].distance;
                nodeNearest = grafo[(int)pos.x][(int)pos.z];
            }
        }

        //We obtain the next position where the enemy has to go
        for (int i = 0; i < shortestDistance-1; ++i)
        {   
               nodeNearest = nodeNearest.previousNode;
        }

        targetPosition = new Vector3(nodeNearest.position.x*4, transform.position.y,nodeNearest.position.z*4) ;
    }

    //Obtain the passables nodes
    void getNodesAdyacentes(int x, int z, ref List<Node> listNodes)
    {

        //Firstly, with x+1, give it priority
        if (x + 1 < width)
        {
            //If this position is not occupied, then we put a new node in the list
            if (!ServersManager.getSingleton().getServer<GameManager>().isTileOccupied(new Vector3((x + 1) * 4, 0, z * 4)))
            {
                grafo[x + 1][z].position = new Vector3(x + 1, transform.position.y, z);
                listNodes.Add(grafo[x + 1][z]);
            }

        }

        //Random position, right or left. In this way, the enemy can change more.  
        System.Random rand = new System.Random();

        int sign = rand.Next(0, 1) * 2 - 1;

        for (int wideCount = -1; wideCount <= 1; ++wideCount)
        {
            for (int WidthCount = 1; WidthCount >= 0; --WidthCount)
            {
                //We check if this position is out
                if (x + WidthCount < width && z + sign * wideCount >= 0 && z + sign * wideCount < wide && wideCount != 0)
                {
                    //We check if this position is occupied by another gameobject
                    if (!ServersManager.getSingleton().getServer<GameManager>().isTileOccupied(new Vector3((x + WidthCount) * 4, 0, (z + sign * wideCount) * 4)))
                    {
                        //Add a new node in the list
                        grafo[x + WidthCount][sign * wideCount + z].position = new Vector3(x + WidthCount, transform.position.y, sign * wideCount + z);
                        listNodes.Add(grafo[x + WidthCount][sign * wideCount + z]);
                    }

                }
            }
        }
    }

    
}
