using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TerrainUtils;

public class ChessGrid : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer mainColor_renderer;

    [SerializeField]
    private Dictionary<string, ChessGrid> neighborGrid = new Dictionary<string, ChessGrid>();

    public Vector2Int hasLeftRightNeighbor = Vector2Int.zero;

    public Vector2Int hasDownUpNeighbor = Vector2Int.zero;

    private int costMask;

    public Vector2Int gridIndex;

    public byte cost = 1;

    public Vector2Int weightDir = Vector2Int.zero;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        costMask = LayerMask.GetMask("Unit", "Obstacle");
    }

    void FixedUpdate()
    {
        Collider2D costObject = Physics2D.OverlapBox(this.transform.position, Vector2.one * 0.5f, 0f, costMask);
        if (costObject == null)
        {
            cost = 1;
            weightDir = Vector2Int.zero;
            return;
        }

        cost = byte.MaxValue;

        if (hasLeftRightNeighbor == new Vector2Int(1,0))    //Only Left neighbor
        {
            weightDir.x = -1;
        }
        else if(hasLeftRightNeighbor == new Vector2Int(0,1))   //Only Right neighbor
        {
            weightDir.x = 1;
        }
        else if(hasLeftRightNeighbor == new Vector2Int(1,1))   //Left & Right neighbor
        {
            //Is there Obstacle?
            //
        }

        if (hasDownUpNeighbor == new Vector2Int(1, 0))  //Only Down neighbor
        {
            weightDir.y = -1;
        }
        else if (hasDownUpNeighbor == new Vector2Int(0, 1)) //Only Up neighbor
        {
            weightDir.y = 1;
        }
        else if (hasDownUpNeighbor == new Vector2Int(1, 1)) //Down & Up neighbor
        {

        }
    }

    public void SetNeighbor(string dir, ChessGrid g)
    {
        neighborGrid.Add(dir, g);
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
