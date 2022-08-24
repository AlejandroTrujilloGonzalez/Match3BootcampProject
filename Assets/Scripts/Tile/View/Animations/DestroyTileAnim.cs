using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTileAnim : IViewAnim
{
    private TileView _tile;

    public DestroyTileAnim(TileView tile)
    {
        _tile = tile;
    }

    public Coroutine PlayAnimation(BoardView board)
    {
        return board.StartCoroutine(AnimationCoroutine(board));
    }

    private IEnumerator AnimationCoroutine(BoardView board)
    {
        board.RemoveTileView(_tile);
        yield return _tile.DestroyTile();
    }
}
