using UnityEngine;

[System.Serializable]
public class ShopProbabilityData
{
    public int probabilityID;
    public int level;

    [Header("-Probabilities")]
    public int cost1;
    public int cost2;
    public int cost3;
    public int cost4;
    public int cost5;

    public int GetProbability(int cost)
    {
        switch (cost)
        {
            case 0: return cost1;
            case 1: return cost2;
            case 2: return cost3;
            case 3: return cost4;
            case 4: return cost5;
        }

        return 0;
    }
}
