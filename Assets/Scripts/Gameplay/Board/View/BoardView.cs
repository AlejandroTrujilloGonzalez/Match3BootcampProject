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
    public Vector2Int boardSize = new Vector2Int(7, 6);

    public Camera inputCamera;    

    private Plane inputPlane;
    private List<TileView> tiles = new List<TileView>();
    public BoardController boardController;
    private List<IViewAnim> animations = new List<IViewAnim>();

    //Level
    public LevelController levelController;
    public LevelListSO levelListSO;
    private LevelSO levelValues;

    public TMP_Text idText;
    public TMP_Text movesText;
    public TMP_Text goalText;

    public EnemyController enemyController;
    public TMP_Text enemyLifeText;
    public GameObject enemy;

    public GameObject losePanel;
    public GameObject winPanel;
    public Button loseAdButton = null;
    public Button winAdButton = null;

    private int nTilesDestroyed = 0;

    //Services
    private GameConfigService _gameConfig;
    private AnalyticsGameService _analytics = null;

    private bool IsAnimating => animations.Count > 0;

    private void Awake()
    {
        if (inputCamera == null)
            inputCamera = Camera.main;

        inputPlane = new Plane(Vector3.forward, Vector3.zero);
        _analytics = ServiceLocator.GetService<AnalyticsGameService>();
        _gameConfig = ServiceLocator.GetService<GameConfigService>();

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
        boardController.ProcessInput(Vector2Int.zero);
    }

    private void Update()
    {
        if (IsAnimating)
            return;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var ray = inputCamera.ScreenPointToRay(Input.mousePosition);
            if (inputPlane.Raycast(ray, out float hitDistance))
            {
                Vector3 hitPosition = ray.GetPoint(hitDistance);
                Debug.Log(hitPosition);
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
        levelValues = levelListSO.levelList[DataController.Instance.data.playerCurrentLevel];
        boardController = new BoardController(levelValues.boardWidth, levelValues.boardHeight);
        boardController.maxTilesTypes = levelValues.maxTilesTypes;
        levelController = new LevelController(levelValues.id, levelValues.moves, levelValues.maxTilesTypes, levelValues.enemy);
        enemyController = new EnemyController(levelValues.enemy.enemyName, levelValues.enemy.life);
        //enemy.GetComponent<SpriteRenderer>().sprite = levelValues.enemy.sprite;
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
        enemyLifeText.text = enemyController.GetLife() + "/" + levelValues.enemy.life;
    }

    private void OnWin()
    {
        winPanel.SetActive(true);
        winAdButton.interactable = ServiceLocator.GetService<AdsGameService>().IsAdReady;
        _analytics.SendEvent("LevelWin");
        levelController.WinLevel(); 
    }

    private void OnLose()
    {
        losePanel.SetActive(true);
        loseAdButton.interactable = ServiceLocator.GetService<AdsGameService>().IsAdReady;
        _analytics.SendEvent("LevelLose");
        levelController.LoseLevel();
    }

    ////////////////////////////////////////////////////////Buttons
    
    public void ContinueRetry()
    {
        SceneLoader.Instance.LoadScene(1);
    }

    public async void BackToMenuRewarded()
    {
        if (await ServiceLocator.GetService<AdsGameService>().ShowAd())
        {
            _analytics.SendEvent("RewardedAdViewed");
            DataController.Instance.data.playerGold = DataController.Instance.data.playerGold + _gameConfig.GoldPerAd;
            SceneLoader.Instance.LoadScene(0);
        }
    }

    public void BackToMenu()
    {
        SceneLoader.Instance.LoadScene(0);
    }

}
