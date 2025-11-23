using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI level;

    [SerializeField]
    private TextMeshProUGUI exp;
    [SerializeField]
    private Slider expSlider;
    public int maxExp = 4;

    [SerializeField]
    private TextMeshProUGUI gold;

    [SerializeField]
    private GameObject[] life;


    public void SetLevelText(int lv)
    {
        level.text = lv.ToString();
    }

    public void SetExpText(int e)
    {
        exp.text = "( " + e + " / " + maxExp + " )";
        expSlider.value = e;
    }

    public void SetMaxExp(int m)
    {
        maxExp = m;
        expSlider.maxValue = maxExp;
    }

    public void SetgoldText(int g)
    {
        gold.text = g.ToString();
    }

    public void DecreaseLife()
    {
        Destroy(life[life.Length - 1]);
    }
}
