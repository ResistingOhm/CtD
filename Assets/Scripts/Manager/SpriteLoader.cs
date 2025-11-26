using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteLoader : MonoBehaviour
{
    void Start()
    {
        Sprite[] allSprites = Resources.LoadAll<Sprite>("UnitSprite");

        Dictionary<string, List<Sprite>> spriteGroups = new Dictionary<string, List<Sprite>>();

        // 3. 불러온 모든 스프라이트를 반복하며 그룹별로 분류합니다.
        foreach (Sprite sprite in allSprites)
        {
            // 스프라이트 이름(예: "Sprite_1_1")에서 마지막 "_" 이후 부분을 제거합니다.
            // 이렇게 하면 그룹명(예: "Sprite_1")만 남게 됩니다.
            string[] nameParts = sprite.name.Split('_');
            string groupName = nameParts[0] + "_" + nameParts[1];

            // 딕셔너리에 해당 그룹이 없으면 새로 추가합니다.
            if (!spriteGroups.ContainsKey(groupName))
            {
                spriteGroups.Add(groupName, new List<Sprite>());
            }
            // 스프라이트를 해당 그룹 리스트에 추가합니다.
            spriteGroups[groupName].Add(sprite);
        }

        foreach (var group in spriteGroups)
        {
            // 스프라이트 이름을 기준으로 오름차순 정렬합니다.
            List<Sprite> sortedGroup = group.Value.OrderBy(s => s.name).ToList();

            // 정렬된 리스트를 배열로 변환하여 groupedSprites에 추가합니다.
            DataManager.unitSpriteData.Add(sortedGroup.ToArray());
        }

        // 5. 결과 확인 (디버그 용도)
        Debug.Log("총 " + DataManager.unitSpriteData.Count + "개의 스프라이트 그룹을 찾았습니다.");
        for (int i = 0; i < DataManager.unitSpriteData.Count; i++)
        {
            Debug.Log("그룹 " + (i + 1) + "에 포함된 스프라이트 수: " + DataManager.unitSpriteData[i].Length);
            foreach (Sprite s in DataManager.unitSpriteData[i])
            {
                Debug.Log("- " + s.name);
            }
        }
    }
}
