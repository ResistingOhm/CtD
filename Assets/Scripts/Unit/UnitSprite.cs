using UnityEngine;
using UnityEngine.UI;

public enum MovDir
{
    UPI,
    UPM,
    LEFTI,
    LEFTM,
    RIGHTI,
    RIGHTM,
    DOWNI,
    DOWNM,
}

public class UnitSprite : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private Sprite[] sprites;

    public GameObject hpBarPrefab;

    public HpBar hpBar;

    void Update()
    {
        hpBar.GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(0, 0.5f, 0));
    }

    public void InitialSetting(int id, int lv, int maxHp)
    {
        if (hpBar == null)
        {
            hpBar = Instantiate(hpBarPrefab, GameManager.Instance.unitHpContainer).GetComponent<HpBar>();
        }

        hpBar.gameObject.SetActive(true);

        SetUnitSprite(id);

        SetLevelSprite(lv);

        RefreshHp(maxHp);
    }

    private void SetUnitSprite(int id)
    {
        sprites = DataManager.unitSpriteData[id];
    }

    public void SetMoveSprite(MovDir n)
    {
        switch (n)
        {
            case MovDir.UPI:
                spriteRenderer.sprite = sprites[1];
                spriteRenderer.flipX = false;
                break;
            case MovDir.LEFTI:
                spriteRenderer.sprite = sprites[2];
                spriteRenderer.flipX = false;
                break;
            case MovDir.RIGHTI:
                spriteRenderer.sprite = sprites[1];
                spriteRenderer.flipX = true;
                break;
            case MovDir.DOWNI:
                spriteRenderer.sprite = sprites[0];
                spriteRenderer.flipX = false;
                break;
            case MovDir.UPM:
                spriteRenderer.sprite = sprites[4];
                spriteRenderer.flipX = false;
                break;
            case MovDir.LEFTM:
                spriteRenderer.sprite = sprites[5];
                spriteRenderer.flipX = false;
                break;
            case MovDir.RIGHTM:
                spriteRenderer.sprite = sprites[5];
                spriteRenderer.flipX = true;
                break;
            case MovDir.DOWNM:
                spriteRenderer.sprite = sprites[3];
                spriteRenderer.flipX = false;
                break;
                
        }
    }

    public void SetLevelSprite(int lv)
    {
        hpBar.SetLevelColor(lv);
    }

    public void RefreshHp(int mxHp)
    {
        hpBar.RefreshHp(mxHp);
    }

    public void SetCurrentHp(int hp)
    {
        hpBar.SetCurrentHp(hp);
    }

    public void SetVisibility(bool b)
    {
        spriteRenderer.enabled = b;
        hpBar.gameObject.SetActive(b);
    }
}
