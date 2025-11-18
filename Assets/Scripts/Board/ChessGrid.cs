using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TerrainUtils;

public class ChessGrid : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer mainColor_renderer;

    private List<ChessGrid> neighborGrid = new List<ChessGrid>();
    private List<ChessGrid> neighborDiagonalGrid = new List<ChessGrid>();
    public DroppableTile droppableObject { get; private set; }

    public Vector2Int gridIndex;

    public byte cost = 1;

    public int fCost = 0;

    public int gCost = 0;

    public ChessGrid parentGrid;

    void Start()
    {
        droppableObject = GetComponent<DroppableTile>();
    }

    public void SetNeighbor(ChessGrid g, bool tag)
    {
        switch (tag)
        {
            case true:
                neighborGrid.Add(g);
                break;
            case false:
                neighborDiagonalGrid.Add(g);
                break;
        }
    }

    public List<ChessGrid> GetNeighbor(bool tag)
    {
        switch (tag)
        {
            case true:
                return neighborGrid;
            case false:
                return neighborGrid.Concat(neighborDiagonalGrid).ToList();
        }
    }

    public void SetMainColor(Color color)
    {
        mainColor_renderer.color = color;
    }

    public void SetIndex(int x, int y)
    {
        gridIndex = new Vector2Int(x, y);
    }

    public void ResetNodeData()
    {
        fCost = 0;
        gCost = 0;
        parentGrid = null;
    }

    public void NowFilled(DraggableObject g)
    {
        cost = byte.MaxValue;
        if (droppableObject == null) return;
        droppableObject.canAccept = false;
        droppableObject.objectNow = g;
    }

    public void NowEmpty()
    {
        cost = 1;
        if (droppableObject == null) return;
        droppableObject.canAccept = true;
        droppableObject.objectNow = null;
    }

    public void Refresh()
    {
        if (droppableObject == null) return;
        if (droppableObject.objectNow != null)
        {
            cost = byte.MaxValue;
        }
    }

    public int GetDistance(ChessGrid grid)
    {
        Vector2Int dirVec = grid.gridIndex - gridIndex;

        return Mathf.Abs(dirVec.x) + Mathf.Abs(dirVec.y);
    }
}

