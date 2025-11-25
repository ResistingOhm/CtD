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

    public SynergyUIManager synergyUIManager;

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
        CalculateSynergy(u.GetUnitID(), true);

        units.Add(u);
        u.isDeck = true;

        foreach (var v in units)
        {
            v.RefreshStatus();
        }
    }

    public void RemoveUnitFromField(Unit u)
    {
        CalculateSynergy(u.GetUnitID(), false);

        units.Remove(u);
        u.isDeck = false;

        foreach (var v in units)
        {
            v.RefreshStatus();
        }
    }

    private void CalculateSynergy(int unitId, bool isAdd)
    {
        if (isAdd)
        {
            foreach (var u in units)
            {
                if (u.GetUnitID() == unitId) return;
            }
        }


        var synergy = DataManager.unitData[unitId].unitSynergy;
        for (int i = 0; i < synergy.Length; i++)
        {
            int id = synergy[i];

            if (!unitSynergy.ContainsKey(id)) unitSynergy.Add(id, new int[2] { 0, 0 });

            unitSynergy[id][0] += isAdd?  1:-1;


            if (unitSynergy[id][0] < 0)
            {
                unitSynergy[id][0] = 0;
            }

            for (int j = DataManager.unitSynergyData[id].requiredCounts.Length - 1; j >= 0; j--)
            {
                if (unitSynergy[id][0] >= DataManager.unitSynergyData[id].requiredCounts[j])
                {
                    unitSynergy[id][1] = j + 1;
                    break;
                }
                unitSynergy[id][1] = 0;
            }

            if (synergyUIManager != null)
            {
                synergyUIManager.UpdateSynergyUISlot(id, unitSynergy[id]);
            }
        }
        
    }
}
