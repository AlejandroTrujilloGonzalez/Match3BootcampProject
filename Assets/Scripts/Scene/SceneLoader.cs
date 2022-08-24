using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{

    public GameObject loadingBar;
    public Slider slider;
    public TMP_Text progressText;

   
    public void LoadLovel(int sceneId)
    {
        StartCoroutine(LoadAsyncScene(sceneId));
        
    }

    IEnumerator LoadAsyncScene(int sceneId)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneId);

        loadingBar.SetActive(true);

        while (!op.isDone)
        {
            float loadProgress = Mathf.Clamp01(op.progress / .9f);
            slider.value = loadProgress;
            progressText.text = loadProgress * 100f + "%";

            yield return null;
        }
    }
}
