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

    public Vector2Int gridIndex;

    public byte cost = 1;

    public int fCost = 0;

    public int gCost = 0;

    public ChessGrid parentGrid;

    public void SetTile()
    {
        DroppableTile t = GetComponent<DroppableTile>();
        t.filledAction += OnFilled;
        t.emptyAction += OnEmpty;
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

    public void OnFilled()
    {
        cost = byte.MaxValue;
    }

    public void OnEmpty()
    {
        cost = 1;
    }

    public int GetDistance(ChessGrid grid)
    {
        Vector2Int dirVec = grid.gridIndex - gridIndex;

        return Mathf.Abs(dirVec.x) + Mathf.Abs(dirVec.y);
    }
}

