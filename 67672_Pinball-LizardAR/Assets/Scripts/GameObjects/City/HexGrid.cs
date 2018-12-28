using System;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid
{
    #region Properties
    public HexNode Root { get; private set; }
    public List<GameObject> BuildingObjects { get; set; }
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
                Root.gameObject.transform.parent = parent.transform;
                NodeCount = 1;
                mostRecent = Root;

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

    public class HexNode
    {
        #region Properties
        public GameObject gameObject { get; set; }
        public bool IsBuilding { get; set; }
        public HexNode TopLeftNeighbor { get { return Neighbors[0]; } set { Neighbors[0] = value; } }
        public HexNode TopNeighbor { get { return Neighbors[1]; } set { Neighbors[1] = value; } }
        public HexNode TopRightNeighbor { get { return Neighbors[2]; } set { Neighbors[2] = value; } }
        public HexNode BottomRightNeighbor { get { return Neighbors[3]; } set { Neighbors[3] = value; } }
        public HexNode BottomNeighbor { get { return Neighbors[4]; } set { Neighbors[4] = value; } }
        public HexNode BottomLeftNeighbor { get { return Neighbors[5]; } set { Neighbors[5] = value; } }
        public Building GamePlayBuilding {get; set;}
        public Guid NodeId { get; private set; }
        public HexNode[] Neighbors { get; private set; }
        #endregion

        #region Location Constants
        private const float fluctuationDistance = 0.04f;
        private const float standardDistance = 0.045f;
        private const float largerDistance = 0.1f;
        private const float standardHeight = 0.001f;
        private const float depthModifier = 0.75f;
        #endregion

        #region Functionality
        public HexNode()
        {
            Neighbors = new HexNode[]
            {
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
            };
            NodeId = Guid.NewGuid();
        }

        public int FirstEmptyNeighbor()
        {
            for (int i = 0; i < 6; ++i)
            {
                if (Neighbors[i] == null)
                    return i;
            }
            return -1;
        }

        public bool InsertNeighbor(int index, HexNode node, GameObject parent, float depth)
        {
            //this method assumes we are always building out from the root, level by level, won't work for random inserts
            if (Neighbors[index] == null)
            {
                //make sure all our neighbors are correct
                if (index < 0)
                    index = 0;
                else if (index > 5)
                    index = 5;
                //index opposite the one we are assigning
                int complimentaryIndex = index + 3;
                if (complimentaryIndex > 5)
                    complimentaryIndex -= 6;
                Neighbors[index] = node;
                node.Neighbors[complimentaryIndex] = this;
                //adjacent nodes, since this is 6 way after all
                int nextIndex = index + 1;
                if (nextIndex > 5)
                    nextIndex = 0;
                int prevIndex = index - 1;
                if (prevIndex < 0)
                    prevIndex = 5;
                HexNode nextNode = Neighbors[nextIndex];
                HexNode prevNode = Neighbors[prevIndex];
                //make sure the neighbors are correct on other adjacent hexes
                if (nextNode != null)
                {
                    int nextNext = nextIndex + 1;
                    if (nextNext > 5)
                        nextNext = 0;
                    complimentaryIndex = nextNext + 1;
                    if (complimentaryIndex > 5)
                        complimentaryIndex = 0;
                    if (nextNode.Neighbors[complimentaryIndex] == null)
                    {
                        nextNode.Neighbors[complimentaryIndex] = node;
                        node.Neighbors[nextNext] = nextNode;
                    }
                }
                if (prevNode != null)
                {
                    int prevPrev = prevIndex - 1;
                    if (prevPrev < 0)
                        prevPrev = 5;
                    complimentaryIndex = prevPrev - 1;
                    if (complimentaryIndex < 0)
                        complimentaryIndex = 5;
                    if (prevNode.Neighbors[complimentaryIndex] == null)
                    {
                        prevNode.Neighbors[complimentaryIndex] = node;
                        node.Neighbors[prevPrev] = nextNode;
                    }
                }
                //adjust position of game object according to position in data structure
                Vector3 randomVector = new Vector3(UnityEngine.Random.Range(-fluctuationDistance, fluctuationDistance),
                    0, UnityEngine.Random.Range(-fluctuationDistance, fluctuationDistance));
                switch (index)
                {
                    case 0:
                        node.gameObject.transform.position =
                            gameObject.transform.position + new Vector3(-standardDistance, standardHeight, -standardDistance)
                            * (depth * depthModifier) + randomVector;
                        break;
                    case 1:
                        node.gameObject.transform.position =
                            gameObject.transform.position + new Vector3(-standardDistance, standardHeight, standardDistance)
                            * (depth * depthModifier) + randomVector;
                        break;
                    case 2:
                        node.gameObject.transform.position =
                            gameObject.transform.position + new Vector3(largerDistance, standardHeight, 0.0f)
                            * (depth * depthModifier) + randomVector;
                        break;
                    case 3:
                        node.gameObject.transform.position =
                            gameObject.transform.position + new Vector3(standardDistance, standardHeight, standardDistance)
                            * (depth * depthModifier) + randomVector;
                        break;
                    case 4:
                        node.gameObject.transform.position =
                            gameObject.transform.position + new Vector3(standardDistance, standardHeight, -standardDistance)
                            * (depth * depthModifier) + randomVector;
                        break;
                    case 5:
                        node.gameObject.transform.position =
                            gameObject.transform.position + new Vector3(-largerDistance, standardHeight, -0.0f)
                            * (depth * depthModifier) + randomVector;
                        break;
                }
                node.gameObject.transform.parent = parent.transform;
                return true;
            }
            return false;
        }

        public void SpreadExplosion()
        {
            foreach(HexNode neighbor in Neighbors)
            {
                if (neighbor!= null)
                {
                    neighbor.Explode();
                }
            }
        }

        private void Explode()
        {
            GamePlayBuilding.Explode();
        }
        #endregion
    }
}

