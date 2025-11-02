using UnityEngine;
using System.Collections.Generic;
using static ObjectPoolManager;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [SerializeField]
    private ObjectPool unit;
    [SerializeField]
    private ObjectPool item;



    public Dictionary<string, ObjectPool> poolDictionary;

    private void Awake()
    {
        if (Instance == null) Instance = GetComponent<ObjectPoolManager>();
        else Destroy(this.gameObject);

        poolDictionary = new Dictionary<string, ObjectPool>
        {
            { "Unit", unit },
            { "Item", item },

        };
    }

    public T SpawnFromPool<T>(string tag, Vector3 position, Quaternion rotation) where T: MonoBehaviour
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].SpawnObject();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn.GetComponent<T>();
    }

    public T[] SpawnsFromPool<T>(string tag, Vector3 position, Quaternion rotation, int num) where T: MonoBehaviour
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject[] objectToSpawn = new GameObject[num];
        T[] componentToSpawn = new T[num];
        for (int i = 0; i < num; i++)
        {
            objectToSpawn[i] = poolDictionary[tag].SpawnObject();
            objectToSpawn[i].SetActive(true);
            objectToSpawn[i].transform.position = position;
            objectToSpawn[i].transform.rotation = rotation;
            componentToSpawn[i] = objectToSpawn[i].GetComponent<T>();
        }

        return componentToSpawn;
    }


}
