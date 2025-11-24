using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum NodeType
{
    Battle = 0,
    Event = 1,
    Rest = 2,
    Elite = 3,
    Shop = 4,
    Boss = 5,
}

public class MapNode: MonoBehaviour
{
    public int layer;
    public Vector2 position;
    public NodeType type;
    public List<MapNode> toNode;
    public List<MapNode> fromNode;

    public Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnNodeClicked()
    {
        GameManager.Instance.SetCurrentNode(this);
    }

    public void HighlightAvailableConnections()
    {

        foreach (var node in toNode)
        {
            node.button.interactable = true;
        }
        
    }
}
