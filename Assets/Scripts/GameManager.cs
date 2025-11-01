using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isRunning = false;

    public event Action startFight;
    public event Action endFight;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
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
