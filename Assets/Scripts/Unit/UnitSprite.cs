using UnityEngine;
using UnityEngine.UI;

public class UnitSprite : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

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


    }

    public void SetLevelSprite(int lv)
    {
        switch (lv)
        {
            case 0:
                hpBar.SetLevelColor(Color.blue);
                break;
            case 1:
                hpBar.SetLevelColor(Color.gray);
                break;
            case 2:
                hpBar.SetLevelColor(Color.yellow);
                break;
            default:
                hpBar.SetLevelColor(Color.green);
                break;
        }
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
