using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{

    public RectTransform mapPanel;
    public GameObject nodePrefab;
    public GameObject linePrefab;

    private MapNode currentNode = null;

    public void RenderMap(List<List<MapNode>> map)
    {
        foreach (var layer in map)
        {
            foreach (var node in layer)
            {
                var g = Instantiate(nodePrefab, mapPanel);
                var n = g.GetComponent<MapNode>();
                n = node;
                g.transform.localPosition = node.position;
                n.onClick = OnNodeClicked;
            }
        }

        foreach (var layer in map)
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
        var list = mapPanel.GetComponentsInChildren<MapNode>();

        foreach (var ui in list)
        {
            ui.button.interactable =
                currentNode == null || currentNode.toNode.Contains(ui);
        }
    }

    void CreateLine(Vector2 from, Vector2 to)
    {
        GameObject line = Instantiate(linePrefab, mapPanel);
        var lr = line.GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, from);
        lr.SetPosition(1, to);
    }
}
