using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    public GameObject loadingCanvas;
    public Slider slider;
    public TMP_Text progressText;

    private void Awake()
    {

         Time.timeScale = 1; 
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadAsyncScene(sceneId));        
    }

    IEnumerator LoadAsyncScene(int sceneId)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneId);

        loadingCanvas.SetActive(true);

        while (!op.isDone)
        {
            float loadProgress = Mathf.Clamp01(op.progress / .9f);
            slider.value = loadProgress;
            progressText.text = loadProgress * 100f + "%";

            yield return null;
        }

        loadingCanvas.SetActive(false);
    }
}
