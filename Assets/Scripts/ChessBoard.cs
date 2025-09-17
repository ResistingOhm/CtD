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
            for (int y = 0; y < boardSize.y; y++)
            {
                Vector3 worldPos = new Vector3(boardStartPosition.x + gridDiameter * x + gridRadius,boardStartPosition.y +  gridDiameter * y + gridRadius, 0);
                GameObject g = Instantiate(chessGrid, worldPos, Quaternion.identity);
                g.transform.SetParent(this.transform);

                g.GetComponent<ChessGrid>().SetIndex(new Vector2Int(x,y));
                board[x, y] = g.GetComponent<ChessGrid>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
