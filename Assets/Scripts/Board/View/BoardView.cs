using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardView : MonoBehaviour
{
    public Vector2Int boardSize = new Vector2Int(7, 6);

    public Camera inputCamera;    

    private Plane inputPlane;
    private List<TileView> tiles = new List<TileView>();
    public BoardController boardController;
    private List<IViewAnim> animations = new List<IViewAnim>();

    //level
    public LevelController levelController;
    public LevelSO levelValues;

    public TMP_Text idText;
    public TMP_Text movesText;

    private bool IsAnimating => animations.Count > 0;

    private void Awake()
    {
        if (inputCamera == null)
            inputCamera = Camera.main;

        inputPlane = new Plane(Vector3.forward, Vector3.zero);
        boardController = new BoardController(boardSize.x, boardSize.y);
        boardController.OnTileCreated += OnTileCreated;
        boardController.OnTileMoved += OnTileMoved;
        boardController.OnTileDestroyed += OnTileDestroyed;

        levelController = new LevelController(levelValues.id, levelValues.moves);
        UpdateTextMoves();
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

        if (Input.GetMouseButtonDown(0))
        {
            var ray = inputCamera.ScreenPointToRay(Input.mousePosition);
            if (inputPlane.Raycast(ray, out float hitDistance))
            {
                Vector3 hitPosition = ray.GetPoint(hitDistance);
                boardController.ProcessInput(new Vector2Int((int)hitPosition.x, (int)hitPosition.y));

                if (boardController.isMatch)
                {
                    //TODO: MAKE DAMAGE
                    levelController.DecreaseMoves();
                    UpdateTextMoves();
                }
                boardController.isMatch = false;
            }
        }

        if (levelController.GetMoves() <= 0)
        {
            //TODO: GAME OVER
        }

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

}
