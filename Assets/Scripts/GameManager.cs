using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isRunning = false;

    public UnitDeck playerDeck;
    public UnitDeck enemyDeck;

    public event Action startFight;
    public event Action endFight;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    public void Spawn()
    {
        var g = ObjectPoolManager.Instance.SpawnsFromPool<Unit>("Unit", Vector3.zero, Quaternion.identity, 5);
        foreach (var p in g)
        {
            p.tag = "Ally";
            p.SetDeck(playerDeck);
            p.SetUnitData(DataManager.unitData[0]);
        }

        var j = ObjectPoolManager.Instance.SpawnFromPool<Unit>("Unit", Vector3.zero, Quaternion.identity);
        j.tag = "Enemy";
        j.SetDeck(enemyDeck);
        j.SetUnitData(DataManager.unitData[1]);
        enemyDeck.AddUnit(j);
        j.currentPos = ChessBoard.Instance.GetGridFromWorldPos(new Vector2(-6, 3));
        j.transform.position = j.currentPos.transform.position;

        var k = ObjectPoolManager.Instance.SpawnFromPool<Unit>("Unit", Vector3.zero, Quaternion.identity);
        k.tag = "Enemy";
        k.SetDeck(enemyDeck);
        k.SetUnitData(DataManager.unitData[1]);
        enemyDeck.AddUnit(k);
        k.currentPos = ChessBoard.Instance.GetGridFromWorldPos(new Vector2(-1, 1));
        k.transform.position = k.currentPos.transform.position;

    }

    public void StartFight()
    {
        startFight?.Invoke();
    }

    public void EndFight()
    {
        endFight?.Invoke();
    }
}
