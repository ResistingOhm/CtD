using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Net.WebRequestMethods;

public class UnitDeck : MonoBehaviour
{
    public int deckLevel = 1;

    public List<Unit> units = new List<Unit>();

    //Key, Number, Level
    public Dictionary<int, int[]> unitSynergy = new Dictionary<int, int[]>();

    void Start()
    {
        for (int i = 0; i < DataManager.unitSynergyData.Count - 1; i++)
        {
            unitSynergy.Add(i, new int[2] {0,0});
        }
    }

    public bool IsAbleToAddUnit()
    {
        return units.Count < deckLevel;
    }

    public void AddUnit(Unit u)
    {
        units.Add(u);
        var a = u.GetUnitSynergy();
        foreach (var v in a)
        {
            if (!unitSynergy.ContainsKey(v)) unitSynergy.Add(v, new int[2] { 0, 0 });
            unitSynergy[v][0] += 1;
            Debug.Log(unitSynergy[v][0]);
            for (int i = DataManager.unitSynergyData[v].requiredCounts.Length - 1; i >= 0; i--)
            {
                if (unitSynergy[v][0] >= DataManager.unitSynergyData[v].requiredCounts[i])
                {
                    unitSynergy[v][1] = i+1;
                    break;
                }
            }
        }

        foreach (var v in units)
        {
            v.RefreshStatus();
        }
    }

    public void RemoveUnit(Unit u)
    {
        units.Remove(u);
        var a = u.GetUnitSynergy();
        foreach (var v in a)
        {
            unitSynergy[v][0] -= 1;
            for (int i = DataManager.unitSynergyData[v].requiredCounts.Length - 1; i >= 0; i--)
            {
                if (unitSynergy[v][0] >= DataManager.unitSynergyData[v].requiredCounts[i])
                {
                    unitSynergy[v][1] = i + 1;
                    break;
                }
            }
        }

        foreach (var v in units)
        {
            v.RefreshStatus();
        }
    }
}
