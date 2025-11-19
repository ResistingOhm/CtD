using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public bool IsAbleToAddUnitToField()
    {
        return units.Count < deckLevel;
    }

    public void AddUnitToField(Unit u)
    {
        units.Add(u);
        u.isDeck = true;
        var a = u.GetUnitSynergy();
        foreach (var v in a)
        {
            if (!unitSynergy.ContainsKey(v)) unitSynergy.Add(v, new int[2] { 0, 0 });
            unitSynergy[v][0] += 1;
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

    public void RemoveUnitFromField(Unit u)
    {
        units.Remove(u);
        u.isDeck = false;
        var a = u.GetUnitSynergy();
        foreach (var v in a)
        {
            unitSynergy[v][0] -= 1;
            if (unitSynergy[v][0] < 0)
            {
                unitSynergy[v][0] = 0;
            }
            else
            {
                for (int i = DataManager.unitSynergyData[v].requiredCounts.Length - 1; i >= 0; i--)
                {
                    if (unitSynergy[v][0] >= DataManager.unitSynergyData[v].requiredCounts[i])
                    {
                        unitSynergy[v][1] = i + 1;
                        break;
                    }

                    unitSynergy[v][1] = 0;
                }
            }
        }

        foreach (var v in units)
        {
            v.RefreshStatus();
        }
    }
}
