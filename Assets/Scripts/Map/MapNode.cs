using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum NodeType
{
    Battle = 0,
    Shop = 1,
    Rest = 2,
    Event = 3,
    Elite = 4,
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
    public Action<MapNode> onClick;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => onClick?.Invoke(this));
    }
}
