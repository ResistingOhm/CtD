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
                g.GetComponent<ChessGrid>().SetMainColor(isBlack? Color.black: Color.white);
                g.GetComponent<ChessGrid>().SetIndex(x, y);
                board[x, y] = g.GetComponent<ChessGrid>();
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

            foreach (ChessGrid c in openList) //���� openList�� �ٸ� ��尡 �� �ִٸ�, ���ؼ� �ּ��� ��带 ã�´�.
            {
                if (c.fCost <= curGrid.fCost)
                { //������ ��带 Ž���ϴ� ����. fcost�� ���� ���� ��尡 curNode�� �ȴ�.

                    curGrid = c;
                }
            }
            openList.Remove(curGrid);
            closedList.Add(curGrid);  //closedList�� Ž���� �� ���� ��带 �ִ´�.    

            if (curGrid == endGrid)
            {   //�����尡 ��������� Ž���� �����ϰ� endNode���� ���������� Ž���� �����Ѵ�.
                return FindNextGrid(startGrid, curGrid);
            }

            foreach (ChessGrid grid in curGrid.GetNeighbor(tag))
            {
                if (grid.cost == byte.MaxValue) continue;
                if (closedList.Contains(grid)) continue;

                Vector2Int dirVec = grid.gridIndex - endGrid.gridIndex;
                int distance = dirVec.x + dirVec.y;
                int tempg = curGrid.gCost + grid.cost;

                if (tempg < grid.gCost || !openList.Contains(grid))
                { //���¸���Ʈ�� �� ��尡 ���ų�,�־ ���� ���� gcost �� �� ������쿣
                    //gcost�� �ٽ� ����� ������ �־��ش�. ���� par��� ���� �������ش�.
                    grid.gCost = tempg;
                    grid.fCost = grid.gCost + distance;
                    grid.parentGrid = curGrid;

                    if (!openList.Contains(grid)) { openList.Add(grid); }
                }
                
            }
        }
        return null; //���� ��� ��带 Ž���ߴµ� �������� ã�� �� �ϸ� null���� ��ȯ
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

    // Update is called once per frame
    void Update()
    {
        //To Test PathFinding
        if (Input.GetMouseButtonDown(0))
        {
            PathFinding(board[0, 0], board[5, 7], true);
        }
    }
}
