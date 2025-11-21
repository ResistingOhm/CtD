using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isRunning = false;

    public Player player;
    public UnitDeck enemyDeck;

    public List<Unit> tempUnitPool = new List<Unit>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    public void TestEnemySpawn()
    {
        var j = ObjectPoolManager.Instance.SpawnFromPool<Unit>("Unit", Vector3.zero, Quaternion.identity);
        j.InitialSetting(DataManager.unitData[1], 0, enemyDeck, false);
        j.AfterDrop(ChessBoard.Instance.GetGridFromWorldPos(new Vector2(-6, 3)).gameObject);
        
        var k = ObjectPoolManager.Instance.SpawnFromPool<Unit>("Unit", Vector3.zero, Quaternion.identity);
        k.InitialSetting(DataManager.unitData[1], 0, enemyDeck, false);
        k.AfterDrop(ChessBoard.Instance.GetGridFromWorldPos(new Vector2(-1, 1)).gameObject);
    }

    public void SpawnAlly()
    {
        player.AddUnit(0,0,0);
    }

    public void SpawnItem()
    {
        player.AddItem(0, 0);
    }

    public void StartFight()
    {
        tempUnitPool.AddRange(player.GetDeckUnits());
        tempUnitPool.AddRange(enemyDeck.units);

        ChessBoard.Instance.StartFighting();

        foreach (var unit in tempUnitPool)
        {
            unit.StartFighting();
        }

        isRunning = true;

        StartCoroutine(DoFightAction());
    }

    public void EndFight()
    {
        isRunning = false;

        ChessBoard.Instance.EndFighting();

        foreach (var unit in tempUnitPool)
        {
            unit.EndFighting();
        }
        tempUnitPool.Clear();
    }

    IEnumerator DoFightAction()
    {
        WaitForSeconds w = new WaitForSeconds(0.1f);
        while (isRunning)
        {
            foreach (var unit in tempUnitPool)
            {
                unit.DoAction();
            }
            yield return w;
        }
    }
}
