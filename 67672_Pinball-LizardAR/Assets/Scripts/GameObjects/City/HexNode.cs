using System;
using UnityEngine;


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
    public Building GamePlayBuilding { get; set; }
    public Guid NodeId { get; private set; }
    public HexNode[] Neighbors { get; private set; }
    #endregion

    #region Location Constants
    private const float fluctuationDistance = 0.01f;
    private const float standardDistance = 0.04f;
    private const float largerDistance = 0.09f;
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
            {
                index = 0;
            }
            else if (index > 5)
            {
                index = 5;
            }
            //index opposite the one we are assigning
            int complimentaryIndex = index + 3;
            if (complimentaryIndex > 5)
                complimentaryIndex -= 6;
            Neighbors[index] = node;
            node.Neighbors[complimentaryIndex] = this;
            //adjacent nodes, since this is 6 way after all
            int nextIndex = index + 1;
            if (nextIndex > 5)
            {
                nextIndex = 0;
            }
            int prevIndex = index - 1;
            if (prevIndex < 0)
            {
                prevIndex = 5;
            }
            int nodeNextIndex = complimentaryIndex + 1;
            if (nodeNextIndex > 5)
            {
                nodeNextIndex = 0;
            }
            int nodePrevIndex = complimentaryIndex - 1;
            if (nodePrevIndex < 0)
            {
                nodePrevIndex = 5;
            }
            HexNode nextNode = Neighbors[nextIndex];
            HexNode prevNode = Neighbors[prevIndex];
            //make sure the neighbors are correct on other adjacent hexes
            if (nextNode != null)
            {
                int nextCompliment = nodePrevIndex + 3;
                if (nextCompliment > 5)
                {
                    nextCompliment -= 6;
                }
                if (nextNode.Neighbors[nextCompliment] == null)
                {
                    nextNode.Neighbors[nextCompliment] = node;
                    node.Neighbors[nodePrevIndex] = nextNode;
                }
            }
            if (prevNode != null)
            {
                int prevCompliment = nodeNextIndex + 3;
                if (prevCompliment > 5)
                {
                    prevCompliment -= 6;
                }
                if (prevNode.Neighbors[prevCompliment] == null)
                {
                    prevNode.Neighbors[prevCompliment] = node;
                    node.Neighbors[nodeNextIndex] = prevNode;
                }
            }
            //adjust position of game object according to position in data structure
            Vector3 randomVector = new Vector3(UnityEngine.Random.Range(-fluctuationDistance, fluctuationDistance),
                0, UnityEngine.Random.Range(-fluctuationDistance, fluctuationDistance));
            node.gameObject.transform.Rotate(new Vector3(0f, 1f, 0f), UnityEngine.Random.Range(0f, 360f));
            switch (index)
            {
                case 0:
                    node.gameObject.transform.position =
                        gameObject.transform.position + new Vector3(-standardDistance, standardHeight, -standardDistance)
                        * (depth * depthModifier);
                    break;
                case 1:
                    node.gameObject.transform.position =
                        gameObject.transform.position + new Vector3(-standardDistance, standardHeight, standardDistance)
                        * (depth * depthModifier);
                    break;
                case 2:
                    node.gameObject.transform.position =
                        gameObject.transform.position + new Vector3(largerDistance, standardHeight, 0.0f)
                        * (depth * depthModifier);
                    break;
                case 3:
                    node.gameObject.transform.position =
                        gameObject.transform.position + new Vector3(standardDistance, standardHeight, standardDistance)
                        * (depth * depthModifier);
                    break;
                case 4:
                    node.gameObject.transform.position =
                        gameObject.transform.position + new Vector3(standardDistance, standardHeight, -standardDistance)
                        * (depth * depthModifier);
                    break;
                case 5:
                    node.gameObject.transform.position =
                        gameObject.transform.position + new Vector3(-largerDistance, standardHeight, -0.0f)
                        * (depth * depthModifier);
                    break;
            }
            node.gameObject.transform.position += randomVector;
            node.gameObject.transform.parent = parent.transform;
            return true;
        }
        return false;
    }

    public void SpreadExplosion(string damageType)
    {
        foreach (HexNode neighbor in Neighbors)
        {
            if (neighbor != null)
            {
                neighbor.Explode(damageType);
            }
        }
    }

    private void Explode(string damageType)
    {
        if (GamePlayBuilding != null)
        {
            GamePlayBuilding.Explode(damageType);
        }
    }
    #endregion
}

