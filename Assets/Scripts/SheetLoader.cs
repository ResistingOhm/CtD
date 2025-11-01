using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SheetLoader : MonoBehaviour
{
    string sheetUrl = "https://script.google.com/macros/s/AKfycbwgdWAgCmdqrC5hqbjFXomqBqfjZYXk_I_19Ein1jBGyncXZPvEyiGk-6mHRHqy2siy/exec";

    void Start()
    {
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

                Debug.Log(DataManager.unitData);

            }
            else
            {
                Debug.LogError("Error: " + www.error);
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
    }
}
