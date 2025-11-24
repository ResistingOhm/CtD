using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager
{
    private List<Vector2Int> usedPositions = new();
    private Player player;

    public EnemySpawnManager(Player player)
    {
        this.player = player;
    }

    public void Generate(int layer, bool isElite)
    {
        List<Unit> result = new();
        usedPositions.Clear();

        int targetDifficulty = GetStageTargetDifficulty(layer);
        if (isElite) targetDifficulty = Mathf.RoundToInt(targetDifficulty * 1.3f);
        int currentDifficulty = 0;

        while (currentDifficulty < targetDifficulty && result.Count < 10)
        {
            int c = GetRandomCost();
            var candidates = DataManager.unitData.FindAll(u => u.unitCost == c);
            if (candidates.Count == 0)
                continue;

            UnitData unit = candidates[Random.Range(0, candidates.Count)];
            int level = Random.Range(0, 3);

            int difficulty = 2 * c * level;

            // 난이도 초과 방지
            if (currentDifficulty + difficulty > targetDifficulty +5)
                continue;

            Vector2Int pos = GetRandomPos(unit.baseRange);

            GameManager.Instance.SpawnEnemy(unit.unitID, level, pos);

            currentDifficulty += difficulty;
        }
    }

    private int GetRandomCost()
    {
        if (DataManager.shopData == null)
        {
            return 1;
        }

        // playerLevel 범위 보정: 배열은 level 1..N 순으로 들어있다고 가정
        int idx = Mathf.Clamp(player.GetDeckLevel() - 1, 0, DataManager.shopData.Count - 1);
        ShopProbabilityData probs = DataManager.shopData[idx];

        if (probs == null)
        {
            return 1;
        }

        int sum = 0;
        for (int i = 0; i < 5; i++) sum += Mathf.Max(0, probs.GetProbability(i));

        int r = Random.Range(0, sum);
        r += 1;
        int cumulative = 0;
        for (int i = 0; i < 5; i++)
        {
            cumulative += Mathf.Max(0, probs.GetProbability(i));
            if (r < cumulative)
            {
                return i + 1; // index 0 -> 1 cost
            }
        }

        return 1;

    }

    Vector2Int GetRandomPos(int range)
    {
        int y = Mathf.RoundToInt(Mathf.Lerp(4f, 7f, (range - 1) / 4f));

        Vector2Int pos;
        int safety = 0;

        do
        {
            int x = Random.Range(0, 8);
            pos = new Vector2Int(x, y);

            safety++;
            if (safety > 50) break;
        }
        while (usedPositions.Exists(p => Vector2Int.Distance(p, pos) == 0));

        usedPositions.Add(pos);
        return pos;
    }

    int GetStageTargetDifficulty(int stage)
    {
        float t = (stage - 1) / 19f;      // 0~1 보간 인자
        float diff = Mathf.Lerp(3f, 30f, t);

        return Mathf.RoundToInt(diff);
    }
}
