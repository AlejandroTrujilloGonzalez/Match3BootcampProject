using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TileView : MonoBehaviour
{
    public Vector2Int position;

    public virtual Coroutine SpawnTile(Vector2Int newPosition)
    {
        position = newPosition;
        return StartCoroutine(SpawnCoroutine(newPosition));
    }

    private IEnumerator SpawnCoroutine(Vector2Int newPosition)
    {
        yield return new WaitForSeconds(0.01f);
    }

    public virtual Coroutine MoveTo(Vector2Int newPosition)
    {
        return StartCoroutine(MoveToCoroutine(newPosition));
    }

    private IEnumerator MoveToCoroutine(Vector2Int newPosition)
    {
        position = newPosition;
        transform.DOMove(new Vector3(newPosition.x, newPosition.y), 0.05f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.05f);
    }

    public virtual Coroutine DestroyTile()
    {
        return StartCoroutine(DestroyTileCoroutine());
    }

    private IEnumerator DestroyTileCoroutine()
    {
        transform.GetComponent<SpriteRenderer>().DOFade(0f, 0.1f).SetEase(Ease.InOutBounce);
        yield return new WaitForSeconds(0.1f);
        transform.SetParent(null);
        Destroy(gameObject);
    }
}
