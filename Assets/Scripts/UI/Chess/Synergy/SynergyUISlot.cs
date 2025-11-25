using UnityEngine;

public class SynergyUISlot : MonoBehaviour
{
    public int synergyId;
    public int synergyNum;
    public int synergyLv;

    public void InitialSetting(int id)
    {
        synergyId = id;
        synergyNum = 0;
        synergyLv = 0;
    }

    public void SetData(int[] data)
    {
        synergyNum = data[0];
        synergyLv = data[1];
    }

    public void UpdateUI()
    {
        gameObject.SetActive(true);
    }
}
