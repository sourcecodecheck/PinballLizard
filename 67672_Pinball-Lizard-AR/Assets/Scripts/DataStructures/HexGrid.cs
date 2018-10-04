using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.DataStructures
{
    public class HexNode
    {
        #region Properties
        public GameObject gameObject { get; set; }
        public bool isBuilding { get; set; }
        public HexNode TopLeftNeighbor { get { return Neighbors[0]; } set { Neighbors[0] = value; } }
        public HexNode TopNeighbor { get { return Neighbors[1]; } set { Neighbors[1] = value; } }
        public HexNode TopRightNeighbor { get { return Neighbors[2]; } set { Neighbors[2] = value; } }
        public HexNode BottomRightNeighbor { get { return Neighbors[3]; } set { Neighbors[3] = value; } }
        public HexNode BottomNeighbor { get { return Neighbors[4]; } set { Neighbors[4] = value; } }
        public HexNode BottomLeftNeighbor { get { return Neighbors[5]; } set { Neighbors[5] = value; } }
        public Guid NodeId { get; private set; }
        public HexNode[] Neighbors { get; private set; }
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

        public bool InsertNeighbor(int index, ref HexNode node, ref GameObject parent, float depth)
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
                switch (index)
                {
                    case 0:
                        node.gameObject.transform.position = gameObject.transform.position + new Vector3(-0.045f, 0, 0.04f) * (depth * 0.75f);
                        break;
                    case 1:
                        node.gameObject.transform.position = gameObject.transform.position + new Vector3(0.0f, 0, 0.1f) * (depth * 0.75f);
                        break;
                    case 2:
                        node.gameObject.transform.position = gameObject.transform.position + new Vector3(0.045f, 0, 0.04f) * (depth * 0.75f);
                        break;
                    case 3:
                        node.gameObject.transform.position = gameObject.transform.position + new Vector3(0.045f, 0, -0.04f) * (depth * 0.75f);
                        break;
                    case 4:
                        node.gameObject.transform.position = gameObject.transform.position + new Vector3(0.0f, 0, -0.1f) * (depth * 0.75f);
                        break;
                    case 5:
                        node.gameObject.transform.position = gameObject.transform.position + new Vector3(-0.045f, 0, -0.04f) * (depth * 0.75f);
                        break;
                }
                node.gameObject.transform.parent = parent.transform;
                return true;
            }
            return false;
        }
        #endregion
    }
    public class HexGrid
    {
        #region Properties
        public HexNode Root { get; private set; }
        public List<GameObject> BuildingObjects { get; set; }
        public GameObject BlankSpot { get; set; }
        public int NodeCount { get; private set; }
        #endregion

        #region PrivateVariables
        private HexNode mostRecent;
        private int nodeMax;
        #endregion

        #region Functionality
        public HexGrid()
        {
            BuildingObjects = new List<GameObject>();
            BlankSpot = null;
            Root = null;
            nodeMax = 0;
        }

        public void Generate(int amountOfNodes, int seed, float buildingToRoadsRatio, GameObject parent)
        {
            UnityEngine.Random.InitState(seed);
            //check to make sure we have objects in the gameobject pools
            if (BuildingObjects.Count > 0 && BlankSpot != null)
            {
                //increase max by amount of nodes to generate
                nodeMax += amountOfNodes;
                if (Root == null)
                {
                    int building = (int)(UnityEngine.Random.value * (BuildingObjects.Count - 1));
                    Root = new HexNode
                    {
                        isBuilding = true,
                        gameObject = UnityEngine.Object.Instantiate(BuildingObjects[building], parent.transform.position, Quaternion.identity)
                    };
                    Root.gameObject.transform.parent = parent.transform;
                    NodeCount = 1;
                    mostRecent = Root;

                }
                //call recursive helper to fill rest of nodes beyond root
                List<HexNode> nodeQueue = new List<HexNode> { mostRecent };
                List<Guid> visited = new List<Guid>();
                fillNodes(ref nodeQueue, buildingToRoadsRatio, ref visited, ref parent, 1);
            }
            else
                throw new Exception("BuildingObjects must contain entries and BlankSpot must not be null");

        }
        public void Clear()
        {
            Root = null;
        }
        #endregion

        #region Helpers
        private void fillNodes(ref List<HexNode> currentNodeQueue, float buildingToRoadsRatio, ref List<Guid> visited, ref GameObject parent, int depth)
        {
            //initialize next layer's queue
            List<HexNode> nextQueue = new List<HexNode>();
            foreach (HexNode currentNode in currentNodeQueue)
            {
                //save our place
                mostRecent = currentNode;
                //make sure our node hasn't somehow gotten lost
                if (currentNode == null)
                {
                    Debug.Log("This shouldn't happen but just in case");
                    continue;
                }
                //check to make sure we haven't processed this node already
                if (visited.Contains(currentNode.NodeId) == false)
                {
                    visited.Add(currentNode.NodeId);
                    //get the first neighbor of the current node that is null
                    int firstEmpty = currentNode.FirstEmptyNeighbor();
                    bool isNodeBuilding = false;
                    float randomFloat = 0.0f;
                    int objectIndex = 0;
                    //while we have empty neighbors
                    while (firstEmpty != -1)
                    {
                        //pick a gameobject
                        randomFloat = UnityEngine.Random.value;
                        isNodeBuilding = randomFloat < buildingToRoadsRatio;
                        objectIndex = isNodeBuilding ? (int)(UnityEngine.Random.value * (BuildingObjects.Count - 1)) : 0;
                        //create new node
                        HexNode toAdd = new HexNode
                        {
                            isBuilding = isNodeBuilding,
                            gameObject = isNodeBuilding ? UnityEngine.Object.Instantiate(BuildingObjects[objectIndex], parent.transform.position, Quaternion.identity)
                            : UnityEngine.Object.Instantiate(BlankSpot, parent.transform.position, Quaternion.identity)
                        };
                        //attempt to attach node to grid
                        if (currentNode.InsertNeighbor(firstEmpty, ref toAdd, ref parent, depth))
                        {
                            //if successful add node to queue to fill in its neighbors
                            nextQueue.Add(toAdd);
                            //go to next empty on current node
                            firstEmpty = currentNode.FirstEmptyNeighbor();
                            Debug.Log("Node: " + NodeCount.ToString() + " Position: " + toAdd.gameObject.transform.position.ToString());
                            //increment node count
                            ++NodeCount;
                            //if we've reached the desired amount of nodes exit
                            if (NodeCount >= nodeMax)
                                return;
                        }
                        else
                        {
                            UnityEngine.Object.Destroy(toAdd.gameObject);
                        }
                    }
                }
                else
                {
                    //if node has already been processed, look for ones that haven't been
                    nextQueue.AddRange(currentNode.Neighbors);
                }
            }
            //go to next layer of depth and fill nodes there
            fillNodes(ref nextQueue, buildingToRoadsRatio, ref visited, ref parent, depth + 1);
        }
        #endregion
    }
}

