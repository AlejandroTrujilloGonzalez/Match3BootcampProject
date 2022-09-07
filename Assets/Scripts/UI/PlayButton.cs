using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public void ChangeScene(int sceneId)
    {
        SceneLoader.Instance.LoadScene(sceneId);
    }
}
