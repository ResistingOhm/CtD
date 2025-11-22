using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    [Header("Layer Settings")]
    public int totalLayers = 10;
    public int minNodesPerLayer = 3;
    public int maxNodesPerLayer = 4;

    [Header("Seed Settings")]
    public int seed = 0;
    private System.Random rng;

    [Header("Position Settings")]
    public float layerStartY = 50f;
    public float layerSpacingY = 80f;
    public float layerRandY = 20f;
    public float minXSpacing = 100f;
    private float mapWidth;

    [Header("Renderer Settings")]
    public RectTransform mapPanel;
    public RectTransform nodePanel;
    public RectTransform linePanel;
    public GameObject nodePrefab;
    public GameObject linePrefab;

    private MapNode currentNode = null;

    private float finalHeight = 0f;
    private List<List<MapNode>> mapLayerData = new List<List<MapNode>>();

    void Awake()
    {
        if (seed == 0) seed = Random.Range(1, int.MaxValue);
        rng = new System.Random(seed);
    }


    void Start()
    {
        mapWidth = mapPanel.sizeDelta.x - 500f;
        GenerateMap();
        ConnectLayers();
        FixConnections();
        RenderMap();
    }

    private void GenerateMap()
    {
        for (int layer = 0; layer < totalLayers; layer++)
        {
            int nodeCount = (layer == 0 || layer == totalLayers - 1)? 1 : Rand(minNodesPerLayer, maxNodesPerLayer + 1);
            List<MapNode> nodeLayer = new List<MapNode>();

            for (int i = 0; i < nodeCount; i++)
            {
                var g = Instantiate(nodePrefab, nodePanel);
                var node = g.GetComponent<MapNode>();
                node.layer = layer;

                float x = 0f;
                if (layer == 0 || layer == totalLayers - 1)
                {
                    x = 0f;
                }
                else
                {
                    int attempts = 0;
                    bool positionOk = false;

                    while (!positionOk && attempts < 100)
                    {
                        x = RandFloat(-mapWidth / 2f, mapWidth / 2f);
                        positionOk = true;

                        foreach (var n in nodeLayer)
                        {
                            if (Mathf.Abs(n.position.x - x) < minXSpacing)
                            {
                                positionOk = false;
                                break;
                            }
                        }

                        attempts++;
                    }
                }

                if (layer != 0)
                {
                    node.button.interactable = false;
                }

                float y = layerStartY + RandFloat(-layerRandY, layerRandY) + layer * layerSpacingY;
                finalHeight = y;
                node.position = new Vector2(x, y);

                g.GetComponent<RectTransform>().localPosition = node.position;
                //g.transform.localPosition = node.position;
                node.onClick = OnNodeClicked;

                node.type = GetNodeTypeForLayer(layer);
                nodeLayer.Add(node);
            }
            nodeLayer.Sort((a, b) => a.position.x.CompareTo(b.position.x));
            mapLayerData.Add(nodeLayer);
        }

        mapPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(mapWidth, finalHeight + layerStartY);
    }

    private void ConnectLayers()
    {
        var layer0 = mapLayerData[0];
        var layer1 = mapLayerData[1];

        foreach (var node in layer1)
        {
            layer0[0].toNode.Add(node);
            node.fromNode.Add(layer0[0]);
        }

        for (int layer = 1; layer < totalLayers - 1; layer++)
        {
            var current = mapLayerData[layer];
            var next = mapLayerData[layer + 1];

            foreach (var node in current)
            {
                int index = current.IndexOf(node);

                MapNode target = next[Rand(Mathf.Max(0, Mathf.Min(index - 1, next.Count-1)), Mathf.Min(index + 1, next.Count))];

                if (!node.toNode.Contains(target))
                {
                    node.toNode.Add(target);
                    target.fromNode.Add(node);
                }

            }

        }
    }
    private void FixConnections()
    {
        // Ensure all inner layer nodes have incoming and outgoing
        for (int depth = 1; depth < mapLayerData.Count - 1; depth++)
        {
            var prev = mapLayerData[depth - 1];
            var current = mapLayerData[depth];
            var next = mapLayerData[depth + 1];

            foreach (var node in current)
            {
                if (node.fromNode.Count == 0)
                {
                    int index = current.IndexOf(node);

                    MapNode source = prev[Rand(Mathf.Max(0, Mathf.Min(index - 1, prev.Count - 1)), Mathf.Min(index + 1, prev.Count))];
                    source.toNode.Add(node);
                    node.fromNode.Add(source);
                }
            }

            foreach (var node in current)
            {
                if (node.toNode.Count == 0)
                {
                    int index = current.IndexOf(node);

                    MapNode target = next[Rand(Mathf.Max(0, Mathf.Min(index - 1, next.Count - 1)), Mathf.Min(index + 1, next.Count))];
                    node.toNode.Add(target);
                    target.fromNode.Add(node);
                }
            }
        }
    }

    private void RenderMap()
    {
        foreach (var layer in mapLayerData)
        {
            foreach (var node in layer)
            {
                foreach (var target in node.toNode)
                {
                    CreateLine(node.position, target.position);
                }
            }
        }

        HighlightAvailableConnections();
    }

    void OnNodeClicked(MapNode node)
    {
        if (currentNode == null || currentNode.toNode.Contains(node))
        {
            currentNode = node;
            HighlightAvailableConnections();
        }
    }

    void HighlightAvailableConnections()
    {
        if (currentNode == null) return;

        var list = mapPanel.GetComponentsInChildren<MapNode>();

        foreach (var layer in mapLayerData)
        {
            foreach (var node in layer)
            {
                node.button.interactable = currentNode.toNode.Contains(node);
            }

        }
    }

    void CreateLine(Vector2 from, Vector2 to)
    {
        GameObject line = Instantiate(linePrefab, linePanel);
        var lr = line.GetComponent<RectTransform>();

        Vector2 dir = to - from;
        float dist = Vector2.Distance(from, to);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        lr.localPosition = from;
        lr.sizeDelta = new Vector2 (lr.sizeDelta.x, dist);
        lr.rotation = Quaternion.Euler(0, 0, angle - 90f);

    }

    private NodeCluster GetClusterForLayer(int layer)
    {
        foreach (var c in DataManager.nodeClusterData)
            if (layer >= c.startLayer && layer <= c.endLayer)
                return c;
        return DataManager.nodeClusterData[DataManager.nodeClusterData.Count-1];
    }

    private NodeType GetNodeTypeForLayer(int layer)
    {
        if (layer == 0) return NodeType.Battle;
        if (layer == totalLayers - 1) return NodeType.Boss;

        var cluster = GetClusterForLayer(layer);
        int total = 0;

        foreach (var w in cluster.weightedTypes) total += w.weight;
        int roll = Rand(0, total);
        int sum = 0;

        foreach (var w in cluster.weightedTypes)
        {
            sum += w.weight;
            if (roll < sum) return w.type;
        }

        return NodeType.Battle;
    }

    int Rand(int min, int max)
    {
        return rng.Next(min, max);
    }

    float RandFloat(float min, float max)
    {
        return (float)(rng.NextDouble() * (max - min) + min);
    }
}
