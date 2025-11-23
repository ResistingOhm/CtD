using System.Collections.Generic;
using UnityEngine;

public class SynergyUIManager : MonoBehaviour
{
    public int itemsPerPage = 8; // 페이지당 6개 표시
    public int currentPage = 0;

    public Transform synergyContainer;
    public GameObject synergySlotPrefab;

    public GameObject nextButton;

    public Dictionary<int, SynergyUISlot> activeSynergyList = new Dictionary<int, SynergyUISlot>();

    int activeNum = 0;

    void Start()
    {
        foreach(var sd in DataManager.unitSynergyData)
        {
            int id = sd.synergyID;
            if (!activeSynergyList.ContainsKey(id))
            {
                var g = Instantiate(synergySlotPrefab, synergyContainer);
                var slot = g.GetComponent<SynergyUISlot>();
                slot.InitialSetting(id);
                activeSynergyList.Add(id, slot);
            }
        }

        RefreshPage();
    }

    public void UpdateSynergyUISlot(int id, int[] data)
    {
        activeSynergyList[id].SetData(data);
        RefreshPage();
    }

    public void RefreshPage()
    {
        // 페이지 내 슬롯 초기화
        foreach (Transform child in synergyContainer)
            child.gameObject.SetActive(false);

        List<int> activeSynergyId = new List<int>();

        for (int i = 0; i <  activeSynergyList.Count; i++)
        {
            if (activeSynergyList[i].synergyLv > 0) activeSynergyId.Add(activeSynergyList[i].synergyId);
        }

        activeSynergyId.Sort((a, b) =>
        {
            int aLv = activeSynergyList[a].synergyLv;
            int bLv = activeSynergyList[b].synergyLv;

            if (aLv < bLv) return 1;
            if (aLv > bLv) return -1;

            int aNum = activeSynergyList[a].synergyNum;
            int bNum = activeSynergyList[b].synergyNum;

            if (aNum < bNum) return 1;
            if (aNum > bNum) return -1;

            return -1;
        });

        // 페이지에서 보여줄 index 범위 계산
        int start = currentPage * itemsPerPage;
        int end = Mathf.Min(start + itemsPerPage, activeSynergyId.Count);

        for (int i = start; i < end; i++)
        {
            int j = activeSynergyId[i];
            activeSynergyList[j].UpdateUI();
        }

        if (activeSynergyId.Count > itemsPerPage)
        {
            nextButton.SetActive(true);
        }
        else
        {
            nextButton.SetActive(false);
        }
    }

    public void PageConverter()
    {
        int maxPage = Mathf.CeilToInt(activeNum / (float)itemsPerPage) - 1;
        if (currentPage < maxPage)
        {
            currentPage++;
            RefreshPage();
        } else
        {
            currentPage = 0;
            RefreshPage();
        }
    }
}
