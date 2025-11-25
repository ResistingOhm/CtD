using System.Collections.Generic;
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

    public void DecreaseLife(int l)
    {
        var g = life[l];
        var r = g.GetComponent<Rigidbody2D>();
        r.simulated = true;

        r.AddForce(new Vector2(Random.Range(-100,100), 300));
        r.AddTorque(1500);

        Destroy(g, 3f);
    }
}
