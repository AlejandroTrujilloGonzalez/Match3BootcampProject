using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadTest : MonoBehaviour
{
    public PlayerData data;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            DataManager.Save(data);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            data = DataManager.Load();
        }
    }
}
