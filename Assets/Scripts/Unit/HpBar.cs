using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField]
    private Image levelSprite;
    [SerializeField]
    private Slider hpSlider;

    public void SetLevelColor(Color c)
    {
        levelSprite.color = c;
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
