using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTileAnim : IViewAnim
{
    private Vector2Int _from;
    private Vector2Int _to;

    public MoveTileAnim(Vector2Int from, Vector2Int to)
    {
        _from = from;
        _to = to;
    }

    public Coroutine PlayAnimation(BoardView board)
    {
        return board.StartCoroutine(AnimationCoroutine(board));
    }

    private IEnumerator AnimationCoroutine(BoardView board)
    {
        var cell = board.GetTileViewAt(_from);
        if (cell == null)
            yield break;
        yield return cell.MoveTo(_to);
    }
}
