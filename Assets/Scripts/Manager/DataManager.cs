using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public static bool FirstInstall = true;

    public static List<UnitData> unitData = new List<UnitData>();
    public static List<UnitSynergyData> unitSynergyData = new List<UnitSynergyData>();
    public static List<ItemData> itemData = new List<ItemData>();
    public static List<ShopProbabilityData> shopData = new List<ShopProbabilityData>();
    public static List<NodeCluster> nodeClusterData = new List<NodeCluster>();

    public static List<Sprite[]> unitSpriteData = new List<Sprite[]>();
    public static List<Sprite[]> itemSpriteData = new List<Sprite[]>();
}
