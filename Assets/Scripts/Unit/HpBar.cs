using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField]
    private Image levelSprite;

    public Sprite[] level;

    [SerializeField]
    private Slider hpSlider;

    public void SetLevelColor(int i)
    {
        levelSprite.sprite = level[i];
    }

    public void RefreshHp(int mxHp)
    {
        hpSlider.maxValue = mxHp;
        hpSlider.value = mxHp;
    }

    public void SetCurrentHp(int hp)
    {
        hpSlider.value = hp;
    }

}
