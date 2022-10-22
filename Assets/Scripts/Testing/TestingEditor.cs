#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestingScript))]
public class TestingEditor : Editor
{   

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (Application.isPlaying)
        {
            TestingScript myScript = (TestingScript)target;
            if (GUILayout.Button("Reset Level"))
            {
                myScript.SetLevel();
                Debug.Log("Level reset");
            }

            if (GUILayout.Button("Reset energy"))
            {
                myScript.SetEnergy();
                Debug.Log("Energy reset");
            }

            if (GUILayout.Button("Reset gold"))
            {
                myScript.SetGold();
                Debug.Log("Gold reset");
            }
        }
    }
}
#endif
