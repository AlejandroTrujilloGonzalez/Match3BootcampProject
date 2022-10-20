using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Game.Services;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField]
    private GameObject loadingCanvas;
    [SerializeField]
    private Slider loadingSlider;
    [SerializeField]
    private TMP_Text progressText;

    [SerializeField]
    private bool IsDevBuild = true;

    private TaskCompletionSource<bool> _cancellationTaskSource;

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
    }

    //////////////////////////////////////////////Scene Management
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
            loadingSlider.value = loadProgress;
            progressText.text = loadProgress * 100f + "%";

            yield return null;
        }

        loadingCanvas.SetActive(false);
    }

    ////////////////////////////////////////Services management
    void Start()
    {
        _cancellationTaskSource = new();
        LoadServicesCancellable().ContinueWith(task =>
                Debug.LogException(task.Exception),
            TaskContinuationOptions.OnlyOnFaulted);
    }

    private void OnDestroy()
    {
        //_cancellationTaskSource.SetResult(true);
    }

    private async Task LoadServicesCancellable()
    {
        await Task.WhenAny(LoadServices(), _cancellationTaskSource.Task);
    }

    private async Task LoadServices()
    {
        string environmentId = IsDevBuild ? "dev" : "production";

        ServicesInitializer servicesInitializer = new ServicesInitializer(environmentId);

        //create services
        GameConfigService gameConfig = new GameConfigService();
        GameProgressionService gameProgression = new GameProgressionService();

        RemoteConfigGameService remoteConfig = new RemoteConfigGameService();
        LoginGameService loginService = new LoginGameService();
        AnalyticsGameService analyticsService = new AnalyticsGameService();
        AdsGameService adsService = new AdsGameService("4928685", "Rewarded_Android");
        IGameProgressionProvider gameProgressionProvider = new GameProgressionProvider();

        //register services
        ServiceLocator.RegisterService(gameConfig);
        ServiceLocator.RegisterService(gameProgression);
        ServiceLocator.RegisterService(remoteConfig);
        ServiceLocator.RegisterService(loginService);
        ServiceLocator.RegisterService(adsService);
        ServiceLocator.RegisterService(analyticsService);

        //initialize services
        await servicesInitializer.Initialize();
        await loginService.Initialize();
        await remoteConfig.Initialize();
        await analyticsService.Initialize();
        await adsService.Initialize(Application.isEditor);
        await gameProgressionProvider.Initialize();

        gameConfig.Initialize(remoteConfig);
        gameProgression.Initialize(gameConfig, gameProgressionProvider);
    }

}
