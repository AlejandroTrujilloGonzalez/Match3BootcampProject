#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResetLevelScript))]
public class ResetLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (Application.isPlaying)
        {
            ResetLevelScript myScript = (ResetLevelScript)target;
            if (GUILayout.Button("Reset Level"))
            {
                myScript.ResetLevel();
                Debug.Log("Level reset to 0");
            }
        }
    }
}
#endif
