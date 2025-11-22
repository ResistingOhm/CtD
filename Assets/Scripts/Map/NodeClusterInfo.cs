using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeTypeWeight
{
    public NodeType type;
    public int weight;
}

[System.Serializable]
public class NodeCluster
{
    public int clusetID;
    public string clusterName;
    public int startLayer;
    public int endLayer;
    public List<NodeTypeWeight> weightedTypes;
}

