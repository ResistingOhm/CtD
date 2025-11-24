using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isRunning = false;

    public Player player;
    public UnitDeck playerDeck;
    public UnitDeck enemyDeck;

    private List<Unit> tempUnitPool = new List<Unit>();
    private int allyNum;
    private int enemyNum;

    private EnemySpawnManager enemySpawnManager;

    private MapNode currentNode;
    private bool isFirstNode = true;

    public MapUIManager mapUIManager;
    public GameUIManager gameUIManager;

    public Button startButton;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

        enemySpawnManager = new EnemySpawnManager(player);
    }

    public void SpawnEnemy(int id, int level, Vector2Int pos)
    {
        var j = ObjectPoolManager.Instance.SpawnFromPool<Unit>("Unit", Vector3.zero, Quaternion.identity);
        j.InitialSetting(DataManager.unitData[id], level, enemyDeck, false);
        j.AfterDrop(ChessBoard.Instance.GetChessGrid(pos.x, pos.y).gameObject);
    }

    public void SetCurrentNode(MapNode node)
    {
        if (currentNode != null)
        {
            foreach (var n in currentNode.toNode)
            {
                n.button.interactable = false;
            }
        }

        currentNode = node;
        currentNode.button.interactable = false;
        mapUIManager.ToggleMap(false);
        //Fade?
        startButton.interactable = true;

        enemySpawnManager.Generate(node.layer, (node.type == NodeType.Elite));

        if (isFirstNode)
        {
            isFirstNode = false;
            return;
        }
    }

    public void OnStartFightButton()
    {
        allyNum = playerDeck.units.Count;
        enemyNum = enemyDeck.units.Count;

        if (allyNum == 0)
        {
            allyNum = 0;
            enemyNum = 0;
            return;
        }

        startButton.interactable = false;

        tempUnitPool.AddRange(playerDeck.units);
        tempUnitPool.AddRange(enemyDeck.units);


        StartCoroutine(StartBattle());
    }

    private void StartFight()
    {
        ChessBoard.Instance.StartFighting();

        foreach (var unit in tempUnitPool)
        {
            unit.StartFighting();
        }

        isRunning = true;

        StartCoroutine(DoFightAction());
    }

    public void EndFight(bool isWin)
    {
        List<Unit> winner = new List<Unit>();

        if (isWin) winner = playerDeck.units;
        else winner = enemyDeck.units;

        foreach (var unit in winner)
        {
            unit.SetState(unit.winState);
        }

        StartCoroutine(EndBattle(isWin));
    }

    private void ResetUnitsPos(bool isWin)
    {
        isRunning = false;

        ChessBoard.Instance.EndFighting();

        foreach (var unit in tempUnitPool)
        {
            unit.EndFighting();
        }
        tempUnitPool.Clear();
        allyNum = 0;
        enemyNum = 0;

        if (isWin)
        {
            for (int i = enemyDeck.units.Count - 1; i >= 0; i--)
            {
                var e = enemyDeck.units[i];
                enemyDeck.RemoveUnitFromField(e);
                e.DeleteAll();
            }

            player.AddEXP(4);
            player.GoldChange(5);

            mapUIManager.ToggleMap(true);
            currentNode.HighlightAvailableConnections();
        } else
        {
            player.DecreaseLife();
            startButton.interactable = true;
            player.GoldChange(8);
        }
    }

    private IEnumerator DoFightAction()
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

    public void UnitDeadCounter(bool isAlly)
    {
        if (isAlly) allyNum -= 1;
        else enemyNum -= 1;

        if (allyNum <= 0)
        {
            EndFight(false);
        }
        else if (enemyNum <= 0)
        {
            EndFight(true);
        }

    }

    private IEnumerator StartBattle()
    {
        yield return StartCoroutine(gameUIManager.PlayPanelAnimation("전투 시작!"));

        StartFight();
    }

    private IEnumerator EndBattle(bool isWin)
    {
        string resultText = isWin ? "승리!" : "패배...";
        yield return StartCoroutine(gameUIManager.PlayPanelAnimation(resultText));

        ResetUnitsPos(isWin);
    }

}
