using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    public static ChessBoard Instance { get; private set; }

    [SerializeField]
    private GameObject chessGrid;

    private ChessGrid[,] board;
    private float gridRadius = 0.5f;
    private float gridDiameter = 1.0f;

    [SerializeField]
    private Vector2Int boardSize;
    [SerializeField]
    private Vector2Int boardStartPosition;

    private Color black = new Color(50/255f, 50 / 255f, 50 / 255f, 255 / 255f);

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        board = new ChessGrid[boardSize.x, boardSize.y];

        for (int x = 0; x < boardSize.x; x++)
        {
            bool isBlack = x%2==0? true : false;

            for (int y = 0; y < boardSize.y; y++)
            {
                Vector3 worldPos = new Vector3(boardStartPosition.x + gridDiameter * x + gridRadius,boardStartPosition.y +  gridDiameter * y + gridRadius, 0);
                GameObject g = Instantiate(chessGrid, worldPos, Quaternion.identity);
                g.transform.SetParent(this.transform);
                var n = g.GetComponent<ChessGrid>();
                n.SetMainColor(isBlack? black: Color.white);
                n.SetIndex(x, y);
                g.AddComponent(typeof(DroppableTile));
                var t = g.GetComponent<DroppableTile>();
                if (y < boardSize.y / 2)
                {
                    t.SetTag("Ally");
                    t.SetDrag(true);
                }
                else
                {
                    t.SetTag("Enemy");
                    t.SetDrag(false);
                }
                n.SetTile();
                board[x, y] = n;
                isBlack = !isBlack;
            }
        }

        for (int x = 0; x < boardSize.x; x++)
        {
            for (int y = 0; y < boardSize.y; y++)
            {
                if (0 < x)
                {
                    board[x, y].SetNeighbor(board[x - 1, y], true);

                    if (0 < y)
                    {
                        board[x, y].SetNeighbor(board[x - 1, y - 1], false);
                    }
                    if (y < boardSize.y - 1)
                    {
                        board[x, y].SetNeighbor(board[x - 1, y + 1], false);
                    }
                }
                if (x < boardSize.x - 1)
                {
                    board[x, y].SetNeighbor(board[x + 1, y], true);

                    if (0 < y)
                    {
                        board[x, y].SetNeighbor(board[x + 1, y - 1],false);
                    }
                    if (y < boardSize.y - 1)
                    {
                        board[x, y].SetNeighbor(board[x + 1, y + 1], false);
                    }
                }

                if (0 < y)
                {
                    board[x, y].SetNeighbor(board[x, y - 1], true);
                }
                if (y < boardSize.y - 1)
                {
                    board[x, y].SetNeighbor(board[x, y + 1], true);
                }

            }
        }
    }

    public ChessGrid GetGridFromWorldPos(Vector2 worldPos)
    {
        float _x = (worldPos.x - boardStartPosition.x)/gridDiameter;
        float _y = (worldPos.y - boardStartPosition.y)/gridDiameter;

        int x = Mathf.FloorToInt(_x);
        int y = Mathf.FloorToInt(_y);
        return board[x,y];
    }

    public ChessGrid GetChessGrid(int x, int y)
    {
        return board[x,y];
    }

    public ChessGrid PathFinding(ChessGrid s, ChessGrid e, bool tag = true)
    {
        List<ChessGrid> openList = new List<ChessGrid>();
        HashSet<ChessGrid> closedList = new HashSet<ChessGrid>();
        ChessGrid startGrid = s;
        ChessGrid endGrid = e;

        foreach(ChessGrid c in board)
        {
            c.ResetNodeData();
        }

        openList.Add(startGrid); 

        while (openList.Count > 0)
        {
            ChessGrid curGrid = openList[0];

            foreach (ChessGrid c in openList) //만약 openList에 다른 노드가 더 있다면, 비교해서 최선의 노드를 찾는다.
            {
                if (c.fCost <= curGrid.fCost)
                { //최적의 노드를 탐색하는 과정. fcost가 가장 낮은 노드가 curNode가 된다.

                    curGrid = c;
                }
            }
            openList.Remove(curGrid);
            closedList.Add(curGrid);  //closedList에 탐색을 한 현재 노드를 넣는다.    

            if (curGrid == endGrid)
            {   //현재노드가 도착지라면 탐색을 종료하고 endNode부터 역방향으로 탐색을 시작한다.
                return FindNextGrid(startGrid, curGrid);
            }

            foreach (ChessGrid grid in curGrid.GetNeighbor(tag))
            {
                if (grid.cost == byte.MaxValue)
                {
                    if (grid == endGrid) return FindNextGrid(startGrid, curGrid);
                    else continue;
                }
                if (closedList.Contains(grid)) continue;

                int distance = grid.GetDistance(endGrid);
                int tempg = curGrid.gCost + grid.cost;

                if (tempg < grid.gCost || !openList.Contains(grid))
                { //오픈리스트에 위 노드가 없거나,있어도 새로 구한 gcost 가 더 작을경우엔
                    //gcost를 다시 계산한 값으로 넣어준다. 또한 par노드 또한 변경해준다.
                    grid.gCost = tempg;
                    grid.fCost = grid.gCost + distance;
                    grid.parentGrid = curGrid;

                    if (!openList.Contains(grid)) { openList.Add(grid); }
                }
                
            }
        }
        return null; //만약 모든 노드를 탐색했는데 목적지를 찾지 못 하면 null값을 반환
    }

    private ChessGrid FindNextGrid(ChessGrid s, ChessGrid e)
    {
        ChessGrid curGrid = e;
        while (curGrid.parentGrid != s)
        {
            curGrid = curGrid.parentGrid;
        }
        return curGrid;
    }

    public void StartFighting()
    {
        foreach (ChessGrid c in board)
        {
            c.gameObject.GetComponent<DroppableTile>().enabled = false;
        }
    }

    public void EndFighting()
    {
        foreach (ChessGrid c in board)
        {
            c.gameObject.GetComponent<DroppableTile>().enabled = true;
        }
    }
}
