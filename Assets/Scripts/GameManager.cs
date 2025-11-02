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
        var g = ObjectPoolManager.Instance.SpawnsFromPool("Unit", Vector3.zero, Quaternion.identity, 5);
        foreach (var p in g)
        {
            p.tag = "Ally";
            p.GetComponent<Unit>().SetDeck(playerDeck);
            p.GetComponent<Unit>().SetUnitData(DataManager.unitData[0]);
        }

        var j = ObjectPoolManager.Instance.SpawnFromPool("Unit", Vector3.zero, Quaternion.identity);
        j.tag = "Enemy";
        j.GetComponent<Unit>().SetDeck(enemyDeck);
        j.GetComponent<Unit>().SetUnitData(DataManager.unitData[1]);
        enemyDeck.AddUnit(j.GetComponent<Unit>());
        j.GetComponent<Unit>().currentPos = ChessBoard.Instance.GetGridFromWorldPos(new Vector2(-6, 3));
        j.transform.position = j.GetComponent<Unit>().currentPos.transform.position;

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
