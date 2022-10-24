using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Services;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BoardView : MonoBehaviour
{
    //Board
    [SerializeField]
    private Camera inputCamera;    

    private Plane inputPlane;
    private List<TileView> tiles = new List<TileView>();
    private BoardController boardController;
    private List<IViewAnim> animations = new List<IViewAnim>();

    //Level
    private LevelController levelController;
    [SerializeField]
    private LevelListSO levelListSO;
    private LevelSO levelValues;

    [SerializeField]
    private TMP_Text idText;
    [SerializeField]
    private TMP_Text movesText;
    [SerializeField]
    private TMP_Text goalText;

    [SerializeField]
    private EnemyController enemyController;
    [SerializeField]
    private TMP_Text enemyLifeText;
    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private GameObject losePanel;
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private Button loseAdButton = null;
    [SerializeField]
    private Button winAdButton = null;

    private int nTilesDestroyed = 0;

    //Services
    private GameConfigService _gameConfig;
    private AnalyticsGameService _analytics = null;
    private GameProgressionService _gameProgression;

    private bool IsAnimating => animations.Count > 0;

    private void Awake()
    {
        if (inputCamera == null)
            inputCamera = Camera.main;

        inputPlane = new Plane(Vector3.forward, Vector3.zero);
        _analytics = ServiceLocator.GetService<AnalyticsGameService>();
        _gameConfig = ServiceLocator.GetService<GameConfigService>();
        _gameProgression = ServiceLocator.GetService<GameProgressionService>();

        InitializeLevel();
        UpdateTextMoves();
        UpdateEnemyLife();

        boardController.OnTileCreated += OnTileCreated;
        boardController.OnTileMoved += OnTileMoved;
        boardController.OnTileDestroyed += OnTileDestroyed;                
    }

    public void AddTileView(TileView tile)
    {
        tiles.Add(tile);
    }

    public TileView GetTileViewAt(Vector2Int position)
    {
        return tiles.Find(tile => tile.position == position);
    }

    public void RemoveTileView(TileView tile)
    {
        tiles.Remove(tile);
    }

    private void Start()
    {
        AudioPlayer.Instance.PlayRandomBattleMusic();
        boardController.ProcessInput(Vector2Int.zero);
    }

    private void Update()
    {
        if (IsAnimating)
            return;

        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        {
            var ray = inputCamera.ScreenPointToRay(Input.mousePosition);
            if (inputPlane.Raycast(ray, out float hitDistance))
            {
                Vector3 hitPosition = ray.GetPoint(hitDistance);
                boardController.ProcessInput(new Vector2Int((int)hitPosition.x, (int)hitPosition.y));

                if (boardController.isMatch)
                {                  
                    enemyController.TakeDamage(boardController.CalculateDamage(nTilesDestroyed));
                    UpdateEnemyLife();
                    levelController.DecreaseMoves();
                    UpdateTextMoves();
                }
                boardController.isMatch = false;
                nTilesDestroyed = 0;
            }

            if (enemyController.GetLife() <= 0)
            {
                OnWin();
            }
            else if (levelController.GetMoves() <= 0)
            {
                OnLose();
            }
                
        }     
    }

    private void InitializeLevel()
    {
        levelValues = levelListSO.levelList[_gameProgression.CurrentLevel];
        boardController = new BoardController(levelValues.boardWidth, levelValues.boardHeight);
        boardController.maxTilesTypes = levelValues.maxTilesTypes;
        levelController = new LevelController(levelValues.id, levelValues.moves, levelValues.maxTilesTypes, levelValues.enemy);
        enemyController = new EnemyController(levelValues.enemy.enemyName, levelValues.enemy.life);
        enemy.GetComponent<Image>().sprite = levelValues.enemy.sprite;
    }

    private void OnTileCreated(TileModel tile)
    {
        animations.Add(new CreateTileAnim(tile.position, tile.item));
        if (animations.Count == 1)
        {
            StartCoroutine(ProcessAnimations());
        }
    }

    private void OnTileMoved(TileModel from, TileModel to)
    {
        animations.Add(new MoveTileAnim(from.position, to.position));
        if (animations.Count == 1)
        {
            StartCoroutine(ProcessAnimations());
        }
    }

    private void OnTileDestroyed(TileModel tile)
    {
        nTilesDestroyed++;
        animations.Add(new DestroyTileAnim(GetTileViewAt(tile.position)));
        if (animations.Count == 1)
        {
            StartCoroutine(ProcessAnimations());
        }
    }

    private IEnumerator ProcessAnimations()
    {
        while (IsAnimating)
        {
            yield return animations[0].PlayAnimation(this);
            animations.RemoveAt(0);
        }
    }

    private void UpdateTextMoves()
    {
        movesText.text = levelController.GetMoves().ToString();
    }

    //WIP. do with a life bar
    private void UpdateEnemyLife()
    {
        if (enemyController.GetLife() <= 0)
        {
            enemyController.SetLife(0);
        }

        enemyLifeText.text = enemyController.GetLife() + "/" + levelValues.enemy.life;        
    }

    private void OnWin()
    {
        AudioPlayer.Instance.PlaySFX("Win_Level");
        winPanel.SetActive(true);
        winAdButton.interactable = ServiceLocator.GetService<AdsGameService>().IsAdReady;
        _gameProgression.UpdateCurrentLevel(1);
        _gameProgression.UpdateGold(_gameConfig.GoldPerWin);
        _analytics.SendEvent("LevelWin");
    }

    private void OnLose()
    {
        AudioPlayer.Instance.PlaySFX("Lose_Level");
        losePanel.SetActive(true);
        loseAdButton.interactable = ServiceLocator.GetService<AdsGameService>().IsAdReady;
        _gameProgression.UpdateEnergy(-1);
        _analytics.SendEvent("LevelLose");
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    ////////////////////////////////////////////////////////Buttons

    public void ContinueRetry()
    {
        if ((_gameProgression.CurrentLevel >= levelListSO.levelList.Count) || _gameProgression.Energy <= 0)
        {
            SceneLoader.Instance.LoadScene((int)GameplayConstants.mainMenuSceneId);
        } 
        else
        {
            SceneLoader.Instance.LoadScene((int)GameplayConstants.gameplaySceneId);
        }        
    }

    public async void BackToMenuRewarded()
    {
        if (await ServiceLocator.GetService<AdsGameService>().ShowAd())
        {
            _analytics.SendEvent("RewardedAdViewed");           
            _gameProgression.UpdateGold(_gameConfig.GoldPerAd);
            SceneLoader.Instance.LoadScene((int)GameplayConstants.mainMenuSceneId);
        }
    }

    public void BackToMenu()
    {
        SceneLoader.Instance.LoadScene((int)GameplayConstants.mainMenuSceneId);
    }

}
