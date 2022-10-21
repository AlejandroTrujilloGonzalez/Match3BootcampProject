using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalDataController : MonoBehaviour
{
    public static LocalDataController Instance;
    public PlayerLocalData data;
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        data = LocalDataManager.Load();
        LocalDataManager.Save();
    }
    
}
