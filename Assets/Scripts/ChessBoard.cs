using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    [SerializeField]
    private GameObject chessGrid;

    private ChessGrid[,] board;
    private float gridRadius = 0.5f;
    private float gridDiameter = 1.0f;

    [SerializeField]
    private Vector2Int boardSize;
    [SerializeField]
    private Vector2Int boardStartPosition;

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
                if (x > 0)
                {
                    board[x, y].SetNeighbor(board[x - 1, y]);
                }
                if (x < boardSize.x - 1)
                {
                    board[x, y].SetNeighbor(board[x + 1, y]);
                }

                if (y > 0)
                {
                    board[x, y].SetNeighbor(board[x, y - 1]);
                }
                if (y < boardSize.y - 1)
                {
                    board[x, y].SetNeighbor(board[x, y + 1]);
                }

            }
        }
    }

    public void UpdatePathField()
    {
        //Reset Cost
        foreach(ChessGrid g in board)
        {
            g.ResetCost();
        }

        //Give Special Cost to grid

        //Calculate bestcost(Start from destination)
        //g.CreateAllyIntegrationField();
        //g.CreateEnemyIntegrationField();

        //Make Direction to move
        foreach(ChessGrid g in board)
        {
            g.SetUnitDirection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
