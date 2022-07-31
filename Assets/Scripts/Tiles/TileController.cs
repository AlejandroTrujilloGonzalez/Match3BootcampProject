using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Tile tile;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void InitizalizeTile(Tile tile)
    {
        this.tile = tile;
        spriteRenderer.sprite = TileManager.instance.tileSprites[(int) tile.type];
    }

    private void OnMouseDown()
    {
        TileManager.instance.MatchTiles(this);
    }

    private void OnDestroy()
    {
        //TODO: do the damage
    }
}
