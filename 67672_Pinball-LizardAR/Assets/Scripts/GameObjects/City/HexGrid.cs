using System;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid
{
    #region Properties
    public HexNode Root { get; private set; }
    public List<GameObject> BuildingObjects { get; set; }

    public List<GameObject> BuildingInstances { get; set; }
    public GameObject BlankSpot { get; set; }
    public int NodeCount { get; private set; }
    public float MaxX { get; private set; }
    public float MinX { get; private set; }
    public float MaxZ { get; private set; }
    public float MinZ { get; private set; }
    public float XDistance { get { return Vector3.Distance(new Vector3(MinX, 0, 0), new Vector3(MaxX, 0, 0)); } }
    public float ZDistance { get { return Vector3.Distance(new Vector3(MinZ, 0, 0), new Vector3(MaxZ, 0, 0)); } }
    #endregion

    #region PrivateVariables
    private HexNode mostRecent;
    private int nodeMax;
    private int buildingCount;
    #endregion

    #region Functionality
    public HexGrid()
    {
        BuildingObjects = new List<GameObject>();

        BlankSpot = null;
        Root = null;
        nodeMax = 0;
        MaxX = -99999999;
        MinX = 99999999;
        MaxZ = -99999999;
        MinZ = 99999999;
        BuildingInstances = new List<GameObject>();
    }

    public int Generate(int amountOfNodes, int seed, float buildingToEmptyRatio, GameObject parent)
    {
        UnityEngine.Random.InitState(seed);
        
        //check to make sure we have objects in the gameobject pools
        if (BuildingObjects.Count > 0 && BlankSpot != null)
        {
            //increase max by amount of nodes to generate
            nodeMax += amountOfNodes;
            if (Root == null)
            {
                buildingCount = 1;
                int building = UnityEngine.Random.Range(0, BuildingObjects.Count - 1);
                //instantiate root node and set it up
                Root = new HexNode
                {
                    IsBuilding = true,
                    gameObject = UnityEngine.Object.Instantiate(BuildingObjects[building],
                    parent.transform.position, Quaternion.identity)
                };
                Building gamePlayBuilding = Root.gameObject.GetComponent<Building>();
                Root.GamePlayBuilding = gamePlayBuilding;
                gamePlayBuilding.hexNode = Root;
                Root.gameObject.transform.parent = parent.transform;
                NodeCount = 1;
                mostRecent = Root;
                BuildingInstances.Add(Root.gameObject);
            }
            //call recursive helper to fill rest of nodes beyond root
            Queue<HexNode> nodeQueue = new Queue<HexNode>();
            nodeQueue.Enqueue(mostRecent);
            List<Guid> visited = new List<Guid>();
            fillNodes(nodeQueue, buildingToEmptyRatio, visited, parent, 1);
        }
        else
        {
            throw new Exception("BuildingObjects must contain entries and BlankSpot must not be null");
        }

        return buildingCount;
    }

    public Quaternion GetRootRotation()
    {
        return Root.gameObject.transform.rotation;
    }
    public Vector3 GetRootPosition()
    {
        return Root.gameObject.transform.position;
    }
    #endregion

    #region Helpers
    private void fillNodes(Queue<HexNode> currentNodeQueue, float buildingToEmptyRatio,
        List<Guid> visited, GameObject parent, int depth)
    {
        //initialize next layer's queue
        Queue<HexNode> nextQueue = new Queue<HexNode>();
        while (currentNodeQueue.Count != 0)
        {
            HexNode currentNode = currentNodeQueue.Dequeue();
            //save our place
            mostRecent = currentNode;
            //make sure our node hasn't somehow gotten lost
            if (currentNode == null)
            {
                continue;
            }
            //check to make sure we haven't processed this node already
            if (visited.Contains(currentNode.NodeId) == false)
            {
                visited.Add(currentNode.NodeId);
                //get the first neighbor of the current node that is null
                int firstEmpty = currentNode.FirstEmptyNeighbor();
                bool isNodeBuilding = false;
                float buildingDeterminator = 0.0f;
                int buildingIndex = 0;
                //while we have empty neighbors
                while (firstEmpty != -1)
                {
                    //pick a gameobject
                    buildingDeterminator = UnityEngine.Random.value;
                    //determine if node is building or empty
                    isNodeBuilding = buildingDeterminator < buildingToEmptyRatio;

                    buildingIndex = UnityEngine.Random.Range(0, BuildingObjects.Count - 1);
                    //create new node
                    HexNode toAdd = new HexNode
                    {
                        IsBuilding = isNodeBuilding,
                        gameObject = isNodeBuilding ?
                        UnityEngine.Object.Instantiate(BuildingObjects[buildingIndex], parent.transform.position, Quaternion.identity)
                        : UnityEngine.Object.Instantiate(BlankSpot, parent.transform.position, Quaternion.identity)
                    };
                    //To enable certain power ups to work properly
                    if(isNodeBuilding)
                    {
                        Building gamePlayBuilding = toAdd.gameObject.GetComponent<Building>();
                        toAdd.GamePlayBuilding = gamePlayBuilding;
                        gamePlayBuilding.hexNode = toAdd;
                    }
                    //attempt to attach node to grid
                    if (currentNode.InsertNeighbor(firstEmpty, toAdd, parent, depth))
                    {
                        //if successful add node to queue to fill in its neighbors
                        nextQueue.Enqueue(toAdd);
                        //go to next empty on current node
                        firstEmpty = currentNode.FirstEmptyNeighbor();
                        //increment node count
                        ++NodeCount;

                        MaxX = Mathf.Max(MaxX, currentNode.gameObject.transform.position.x);
                        MaxZ = Mathf.Max(MaxZ, currentNode.gameObject.transform.position.z);
                        MinX = Mathf.Min(MinX, currentNode.gameObject.transform.position.x);
                        MinZ = Mathf.Min(MinZ, currentNode.gameObject.transform.position.z);

                        if (isNodeBuilding)
                        {
                            ++buildingCount;
                            BuildingInstances.Add(toAdd.gameObject);
                        }
                        //if we've reached the desired amount of nodes exit
                        if (NodeCount >= nodeMax)
                            return;
                    }
                    else
                    {
                        //destroy object if adding to grid fails
                        UnityEngine.Object.Destroy(toAdd.gameObject);
                    }
                }
            }
            else
            {
                //if node has already been processed, look for ones that haven't been
                foreach (HexNode neighbor in currentNode.Neighbors)
                {
                    if (neighbor != null)
                    {
                        nextQueue.Enqueue(neighbor);
                    }
                }
            }
        }
        //go to next layer of depth and fill nodes there
        fillNodes(nextQueue, buildingToEmptyRatio, visited, parent, depth + 1);
    }

    #endregion

    
}

