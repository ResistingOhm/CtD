using UnityEngine;
using UnityEngine.UI;

public class ChessGrid : MonoBehaviour
{
    [SerializeField]
    private Material material;

    [SerializeField]
    private Color gridMainColor;
    [SerializeField]
    private Color gridLineColor;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
