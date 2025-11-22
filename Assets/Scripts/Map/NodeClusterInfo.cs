using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeCluster
{
    public int clusetID;
    public string clusterName;
    public int startLayer;
    public int endLayer;

    public int battleWeight;
    public int shopWeight;
    public int restWeight;
    public int eventWeight;
    public int eliteWeight;

    public int GetWeight(NodeType type)
    {
        switch (type)
        {
            case NodeType.Battle: return battleWeight;
            case NodeType.Shop: return shopWeight;
            case NodeType.Rest: return restWeight;
            case NodeType.Event: return eventWeight;
            case NodeType.Elite: return eliteWeight;
        }

        return 0;
    }
}

