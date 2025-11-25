using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SheetLoader : MonoBehaviour
{
    string sheetUrl = "https://script.google.com/macros/s/AKfycbwgdWAgCmdqrC5hqbjFXomqBqfjZYXk_I_19Ein1jBGyncXZPvEyiGk-6mHRHqy2siy/exec";

    [SerializeField]
    private GameObject loadingPanel;

    [SerializeField]
    private GameObject errorPanel;

    [SerializeField]
    private LoadingIcon icon;

    void Start()
    {
        if (DataManager.FirstInstall)
        {
            LoadData();
        }
        else
        {
            icon.StopRotation();
            loadingPanel.SetActive(false);
        }
    }

    public void LoadData()
    {
        errorPanel.SetActive(false);
        loadingPanel.SetActive(true);
        StartCoroutine(LoadAllSheets());
    }

    IEnumerator LoadAllSheets()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(sheetUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                var root = JsonUtility.FromJson<Wrapper>(json);

                foreach (var p in root.UnitData)
                    DataManager.unitData.Add(p);
                foreach (var e in root.UnitSynergy)
                    DataManager.unitSynergyData.Add(e);
                foreach (var i in root.ItemData)
                    DataManager.itemData.Add(i);
                foreach (var i in root.ShopProbabilityData)
                    DataManager.shopData.Add(i);
                foreach (var i in root.ClusterData)
                    DataManager.nodeClusterData.Add(i);

                Debug.Log("Succesfully load Data");

                DataManager.FirstInstall = false;

                FadeManager.Instance.FadeOut(() =>
                {
                    icon.StopRotation();
                    loadingPanel.SetActive(false);
                });
            }
            else
            {
                Debug.LogError("Error: " + www.error);
                loadingPanel.SetActive(false);
                errorPanel.SetActive(true);
            }
        }
    }

    // JSON À¯Æ¿
    [System.Serializable]
    public class Wrapper
    {
        public UnitData[] UnitData;
        public UnitSynergyData[] UnitSynergy;
        public ItemData[] ItemData;
        public ShopProbabilityData[] ShopProbabilityData;
        public NodeCluster[] ClusterData;
    }
}
