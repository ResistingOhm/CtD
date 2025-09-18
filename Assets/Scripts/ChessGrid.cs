using System.Collections.Generic;
using UnityEngine;

public class ChessGrid : MonoBehaviour
{
    private SpriteRenderer s_renderer;

    [SerializeField]
    private SpriteRenderer mainColor_renderer;

    [SerializeField]
    private List<ChessGrid> neighborGrid = new List<ChessGrid>();

    public void OnPointerEnter()
    {
        s_renderer.enabled = true;
    }

    public void OnPointerExit()
    {
        s_renderer.enabled = false;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        s_renderer = GetComponent<SpriteRenderer>();
    }

    public void SetNeighbor(ChessGrid g)
    {
        neighborGrid.Add(g);
    }

    public void SetMainColor(Color color)
    {
        mainColor_renderer.color = color;
    }
}

public class CellData
{
    public byte cost;
    public ushort bestCost;
    public Vector2Int dir;

    public CellData()
    {
        this.cost = 1;
        this.bestCost = ushort.MaxValue;
        this.dir = Vector2Int.zero;
    }
}
