using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedTile : TileView
{
    public Animator _animation;

    private bool isAnimating = false;

    public override Coroutine DestroyTile()
    {
        return StartCoroutine(DestroyTileCoroutine());
    }

    public void OnAnimationFinished()
    {
        isAnimating = false;
    }

    private IEnumerator DestroyTileCoroutine()
    {
        isAnimating = true;
        _animation.SetTrigger("kill");
        yield return new WaitUntil(() => isAnimating == false);
        transform.SetParent(null);
        Destroy(gameObject);
    }

}
