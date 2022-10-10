using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CreateTileAnim : IViewAnim
{
    private Vector2Int _position;
    private TileItem _item;

    public CreateTileAnim(Vector2Int position, TileItem item)
    {
        _position = position;
        _item = item;
    }

    public Coroutine PlayAnimation(BoardView board)
    {
        return board.StartCoroutine(AnimationCoroutine(board));
    }

    private IEnumerator AnimationCoroutine(BoardView board)
    {
        AsyncOperationHandle<GameObject> handler =
            Addressables.InstantiateAsync($"Tile_{_item.tileElement}",
                new Vector3(_position.x, _position.y, 0f),
                quaternion.identity,
                board.transform);
        while (!handler.IsDone)
        {
            yield return null;
        }

        TileView view = handler.Result.GetComponent<TileView>();
        board.AddTileView(view);
        yield return view.SpawnTile(_position);
    }

}
