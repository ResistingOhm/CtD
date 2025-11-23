using UnityEngine;

public class UnitSprite : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public void SetUnitSprite(int id)
    {

    }

    public void SetLevelSprite(int lv)
    {
        switch (lv)
        {
            case 0:
                spriteRenderer.color = Color.green;
                break;
            case 1:
                spriteRenderer.color = Color.yellow;
                break;
            case 2:
                spriteRenderer.color = Color.red;
                break;
            default:
                spriteRenderer.color = Color.blue;
                break;
        }
    }

    public void SetVisibility(bool b)
    {
        spriteRenderer.enabled = b;
    }
}
