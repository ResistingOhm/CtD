using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Net.WebRequestMethods;

public class UnitDeck : MonoBehaviour
{
    public List<Unit> units = new List<Unit>();
    public Dictionary<UnitGroup, int> groupSynergy = new Dictionary<UnitGroup, int>();
    public Dictionary<UnitType, int> typeSynergy = new Dictionary<UnitType, int>();

    void Start()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(UnitGroup)).Length - 1; i++)
        {
            groupSynergy.Add((UnitGroup)i, 0);
        }

        for (int i = 0; i < System.Enum.GetValues(typeof(UnitType)).Length - 1; i++)
        {
            typeSynergy.Add((UnitType)i, 0);
        }
    }

    public void AddUnit(Unit u)
    {
        units.Add(u);
        groupSynergy[u.GetUnitGroup()] += 1;
        typeSynergy[u.GetUnitType()] += 1;
    }
}
