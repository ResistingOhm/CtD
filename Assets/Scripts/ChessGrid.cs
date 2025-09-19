using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TerrainUtils;

public class ChessGrid : MonoBehaviour
{
    private SpriteRenderer s_renderer;

    [SerializeField]
    private SpriteRenderer mainColor_renderer;

    [SerializeField]
    private List<ChessGrid> neighborGrid = new List<ChessGrid>();

    private int obstacleMask;

    public Vector2Int gridIndex;

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
        obstacleMask = LayerMask.GetMask("Unit", "Obstacle");
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

    public void ResetCost()
    {
        //Reset
        allyMap.ResetToDefault();
        enemyMap.ResetToDefault();
        /*
        switch (tag)
        {
            case "Enemy":
                allyMap.bestCost = 0;
                allyMap.target = obstacles.gameObject;
                enemyMap.cost = byte.MaxValue;
                break;
            case "Ally":
                enemyMap.bestCost = 0;
                enemyMap.target = obstacles.gameObject;
                allyMap.cost = byte.MaxValue;
                break;
            case "Obstacle":
                enemyMap.cost = byte.MaxValue;
                allyMap.cost = byte.MaxValue;
                break;
            default:
                break;
        }
        */
    }

    public void SetUnitDirection()
    {
        //Give Unit direction
        ushort bestPath_a = allyMap.bestCost;
        ushort bestPath_e = enemyMap.bestCost;
        foreach (ChessGrid g in neighborGrid)
        {
            if (g.allyMap.bestCost < bestPath_a)
            {
                bestPath_a = g.allyMap.bestCost;
                allyMap.target = g.allyMap.target;
                allyMap.dir = g.gridIndex - this.gridIndex;
            }

            if (g.enemyMap.bestCost < bestPath_e)
            {
                bestPath_e = g.enemyMap.bestCost;
                enemyMap.target = g.enemyMap.target;
                enemyMap.dir = g.gridIndex - this.gridIndex;
            }
        }
    }

    public void CreateAllyIntegrationField()
    {
        foreach (ChessGrid g in neighborGrid) {
            CellData a = g.allyMap;
            if (a.cost == byte.MaxValue) { continue; }
            if ((int)a.bestCost + a.cost < allyMap.bestCost)
            {
                allyMap.bestCost = (ushort)(a.bestCost + a.cost);
                continue;
            }
            g.CreateAllyIntegrationField();
        }
    }

    public void CreateEnemyIntegrationField()
    {
        foreach (ChessGrid g in neighborGrid)
        {
            CellData e = g.enemyMap;
            if (e.cost == byte.MaxValue) { continue; }
            if ((int)e.bestCost + e.cost < enemyMap.bestCost)
            {
                enemyMap.bestCost = (ushort)(e.bestCost + e.cost);
                continue;
            }
            g.CreateEnemyIntegrationField();
        }
    }

    public void CostEnter(GameObject u, string tag)
    {
        switch (tag)
        {
            
        }
    }
}

public class CellData
{
    public byte cost;
    public ushort bestCost;
    public Vector2Int dir;
    public GameObject target;

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
