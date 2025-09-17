using UnityEngine;
using UnityEngine.EventSystems;

public class ChessGrid : MonoBehaviour
{
    private SpriteRenderer s_renderer;

    [SerializeField]
    private SpriteRenderer mainColor_renderer;

    private Vector2Int gridIndex;

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

    public void SetIndex(Vector2Int index)
    {
        gridIndex = index;
    }

    public void SetMainColor(Color color)
    {
        mainColor_renderer.color = color;
    }
}
