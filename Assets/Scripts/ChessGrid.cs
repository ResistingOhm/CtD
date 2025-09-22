using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TerrainUtils;

public class ChessGrid : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer mainColor_renderer;

    [SerializeField]
    private List<ChessGrid> neighborGrid = new List<ChessGrid>();

    private int costMask;

    public Vector2Int gridIndex;

    public byte cost = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        costMask = LayerMask.GetMask("Unit", "Obstacle");
    }

    void FixedUpdate()
    {
        Collider2D costObject = Physics2D.OverlapBox(this.transform.position, Vector2.one * 0.5f, 0f, costMask);
        if (costObject != null) { cost = byte.MaxValue; return; }
        cost = 1;
    }

    public void SetNeighbor(ChessGrid g)
    {
        neighborGrid.Add(g);
    }

    public void SetMainColor(Color color)
    {
        mainColor_renderer.color = color;
    }

    public void SetIndex(int x, int y)
    {
        gridIndex = new Vector2Int(x, y);
    }
}
