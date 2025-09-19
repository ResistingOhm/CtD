using System.Collections.Generic;
using UnityEngine;

public class ChessGrid : MonoBehaviour
{
    private SpriteRenderer s_renderer;

    [SerializeField]
    private SpriteRenderer mainColor_renderer;

    [SerializeField]
    private List<ChessGrid> neighborGrid = new List<ChessGrid>();

    public CellData allyMap = new CellData();
    public CellData enemyMap = new CellData();

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

    void FixedUpdate()
    {
        //Set Target and Give Cost to CellData


        //Make IntegrationField
        foreach(ChessGrid g in neighborGrid)
        {
            CreateAllyIntegrationField(g.allyMap);
            CreateEnemyIntegrationField(g.enemyMap);
        }

        //Give Unit direction
        foreach(ChessGrid g in neighborGrid)
        {
            ushort bestPath = 0;
        }
    }

    private void CreateAllyIntegrationField(CellData a)
    {
        if (a.cost == byte.MaxValue) { return; }
        if (a.bestCost + a.cost < allyMap.bestCost)
        {
            allyMap.bestCost = (ushort)(a.bestCost + a.cost);
            allyMap.target = a.target;
        }
    }

    private void CreateEnemyIntegrationField(CellData e)
    {
        if (e.cost == byte.MaxValue) { return; }
        if (e.bestCost + e.cost < enemyMap.bestCost)
        {
            enemyMap.bestCost = (ushort)(e.bestCost + e.cost);
            enemyMap.target = e.target;
        }
    }
}

public class CellData
{
    public byte cost;
    public ushort bestCost;
    public Vector2Int dir;
    public Unit target;

    public CellData()
    {
        this.cost = 1;
        this.bestCost = ushort.MaxValue;
        this.dir = Vector2Int.zero;
        this.target = null;
    }

    public void ResetToDefault()
    {
        this.cost = 1;
        this.bestCost = ushort.MaxValue;
        this.dir = Vector2Int.zero;
        this.target = null;
    }
}
