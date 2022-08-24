using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Tile tile;

    public List<TileController> validNeighbours = new List<TileController>();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void InitizalizeTile(Tile tile)
    {
        this.tile = tile;
        spriteRenderer.sprite = TileManager.instance.tileSprites[(int) tile.type];
        StartCoroutine(CheckNeighborsDelayed());

    }

    private IEnumerator CheckNeighborsDelayed()
    {
        yield return new WaitForSeconds(1);
        validNeighbours = TileManager.instance.GetTileNeighbors(this);
    }

    private void OnMouseDown()
    {
        TileManager.instance.OnClickTile(this);
    }

    private void OnDestroy()
    {
        //TODO: do the damage
    }
}
