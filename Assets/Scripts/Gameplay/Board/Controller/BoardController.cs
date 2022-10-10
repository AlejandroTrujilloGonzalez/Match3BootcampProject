using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController
{

    private BoardModel model;

    public event Action<TileModel> OnTileCreated = delegate (TileModel model) { };
    public event Action<TileModel> OnTileDestroyed = delegate (TileModel model) { };
    public event Action<TileModel, TileModel> OnTileMoved = delegate (TileModel from, TileModel to) { };

    public bool isMatch = false;
    public int maxTilesTypes = 0;

    public BoardController(int width, int height, TileItem[,] initialValues = null)
    {
        model = new BoardModel(width, height, initialValues);
    }

    public void ProcessInput(Vector2Int touchedPos)
    {
        if (touchedPos.x >= 0 && touchedPos.y >= 0 && touchedPos.y < model.Height && touchedPos.x < model.Width)
        {
            isMatch = true;
            ProcessMatches(touchedPos);
        }

        ProcessCollapse();
    }

    private void ProcessMatches(Vector2Int touchedPos)
    {
        TileModel touchedTile = model[touchedPos];
        if (touchedTile.item == null)
            return;

        if (touchedTile.item.tileElement >= 0 && touchedTile.item.tileElement <= maxTilesTypes)
        {
            ProcessColorMatch(touchedTile);
            return;
        }

        if (TryFireArrowBomb(touchedTile))
        {
            return;
        }            

        if (touchedTile.item.tileElement == (int)GameplayConstants.bomb)
        {
            FireBomb(touchedTile);
            return;
        }

        if (touchedTile.item.tileElement == (int)GameplayConstants.horizontalArrow || touchedTile.item.tileElement == (int)GameplayConstants.verticalArrow)
        {
            FireArrow(touchedTile);
            return;
        }

    }

    private void ProcessCollapse()
    {
        for (int y = 0; y < model.Height; ++y)
        {
            for (int x = 0; x < model.Width; ++x)
            {
                if (!model[x, y].IsEmpty())
                    continue;

                int nextY = y;
                while (nextY < model.Height)
                {
                    nextY++;
                    if (nextY == model.Height)
                    {
                        model[x, nextY - 1].item = new TileItem()
                        {
                            tileElement = UnityEngine.Random.Range(0, maxTilesTypes + 1)
                        };
                        OnTileCreated(model[x, nextY - 1]);
                        if (y < nextY - 1)
                        {
                            model[x, y].item = model[x, nextY - 1].item;
                            model[x, nextY - 1].item = null;
                            OnTileMoved(model[x, nextY - 1], model[x, y]);
                        }

                        break;
                    }

                    if (!model[x, nextY].IsEmpty())
                    {
                        model[x, y].item = model[x, nextY].item;
                        model[x, nextY].item = null;
                        OnTileMoved(model[x, nextY], model[x, y]);
                        break;
                    }
                }
            }
        }
    }

    private List<TileModel> GetMatchedTiles(TileModel touchedTile, List<int> extraAllowedMatches)
    {
        List<TileModel> closed = new List<TileModel>();
        if (touchedTile.IsEmpty())
            return closed;

        List<TileModel> open = new List<TileModel>();
        open.Add(touchedTile);
        while (open.Count > 0)
        {
            TileModel tileModel = open[0];
            open.RemoveAt(0);
            closed.Add(tileModel);

            if (tileModel.position.x > 0)
            {
                TileModel neighbour = model[tileModel.position.x - 1, tileModel.position.y];
                ProcessNeighbour(touchedTile, neighbour, extraAllowedMatches, open, closed);
            }

            if (tileModel.position.x < model.Width - 1)
            {
                TileModel neighbour = model[tileModel.position.x + 1, tileModel.position.y];
                ProcessNeighbour(touchedTile, neighbour, extraAllowedMatches, open, closed);
            }

            if (tileModel.position.y > 0)
            {
                TileModel neighbour = model[tileModel.position.x, tileModel.position.y - 1];
                ProcessNeighbour(touchedTile, neighbour, extraAllowedMatches, open, closed);
            }

            if (tileModel.position.y < model.Height - 1)
            {
                TileModel neighbour = model[tileModel.position.x, tileModel.position.y + 1];
                ProcessNeighbour(touchedTile, neighbour, extraAllowedMatches, open, closed);
            }
        }

        return closed;

    }

    private static void ProcessNeighbour(TileModel touchedTile, TileModel neighbour, List<int> extraAllowedMatches,
            List<TileModel> open, List<TileModel> closed)
    {
        if (!neighbour.IsEmpty() &&
            (touchedTile.item.tileElement == neighbour.item.tileElement ||
             extraAllowedMatches.Contains(neighbour.item.tileElement)) &&
            !open.Contains(neighbour) &&
            !closed.Contains(neighbour))
        {
            open.Add(neighbour);
        }
    }

    private void ProcessColorMatch(TileModel touchedTile)
    {
        List<TileModel> matchedTiles = GetMatchedTiles(touchedTile, new List<int>());
        
        if (matchedTiles.Count >= (int)GameplayConstants.nMinimunMatch)
        {
            foreach (TileModel tile in matchedTiles)
            {
                OnTileDestroyed(tile);
                model[tile.position].item = null;
            }

            if (matchedTiles.Count > (int)GameplayConstants.nBombMatch)
            {
                model[touchedTile.position].item = new TileItem { tileElement = (int)GameplayConstants.bomb };
                OnTileCreated(model[touchedTile.position]);
            }
            else if (matchedTiles.Count > (int)GameplayConstants.nArrowMatch)
            {
                model[touchedTile.position].item = new TileItem
                {
                    tileElement = UnityEngine.Random.Range(0, 100) < 50 ? (int)GameplayConstants.horizontalArrow : (int)GameplayConstants.verticalArrow
                };
                OnTileCreated(model[touchedTile.position]);
            }
        }
    }

    private void DestroyTileAt(int x, int y)
    {
        if (x < 0 || y < 0 || x >= model.Width || y >= model.Height)
            return;

        TileModel tile = model[x, y];
        if (!tile.IsEmpty())
        {
            OnTileDestroyed(tile);
            model[x, y].item = null;
        }
    }

    public int CalculateDamage(int nTilesDestroyed)
    {
        int damage = nTilesDestroyed * 10;
        
        return damage;
    }

    //////////////////////////////// SPECIAL MATCHES ///////////////////////////////////

    private void FireArrow(TileModel touchedTile)
    {
        List<TileModel> matchedTiles = GetMatchedTiles(touchedTile, new List<int> { (int)GameplayConstants.horizontalArrow, (int)GameplayConstants.verticalArrow });

        if (touchedTile.item.tileElement == (int)GameplayConstants.horizontalArrow)
        {
            FireHorizontalArrow(touchedTile);
            if (matchedTiles.Count > 1)
            {
                FireVerticalArrow(touchedTile);
            }
        }
        else if (touchedTile.item.tileElement == (int)GameplayConstants.verticalArrow)
        {
            FireVerticalArrow(touchedTile);
            if (matchedTiles.Count > 1)
            {
                FireHorizontalArrow(touchedTile);
            }
        }
    }

    private void FireHorizontalArrow(TileModel touchedTile)
    {
        for (int x = 0; x < model.Width; ++x)
        {
            DestroyTileAt(x, touchedTile.position.y);
        }
    }

    private void FireVerticalArrow(TileModel touchedTile)
    {
        for (int y = 0; y < model.Height; ++y)
        {
            DestroyTileAt(touchedTile.position.x, y);
        }
    }

    private void FireBomb(TileModel touchedTile)
    {
        List<TileModel> matchedTiles = GetMatchedTiles(touchedTile, new List<int>());
        int bombRange = matchedTiles.Count > 1 ? 2 : 1;

        for (int y = -bombRange; y <= bombRange; ++y)
        {
            for (int x = -bombRange; x <= bombRange; ++x)
            {
                DestroyTileAt(touchedTile.position.x + x, touchedTile.position.y + y);
            }
        }
    }

    private bool TryFireArrowBomb(TileModel touchedTiles)
    {
        List<TileModel> matchedTiles = GetMatchedTiles(touchedTiles, new List<int> { (int)GameplayConstants.bomb, (int)GameplayConstants.horizontalArrow, (int)GameplayConstants.verticalArrow });
        if (matchedTiles.Count < 2)
            return false;

        bool hasArrow = false;
        bool hasBomb = false;
        foreach (TileModel tile in matchedTiles)
        {
            hasArrow |= tile.item.tileElement == (int)GameplayConstants.horizontalArrow || tile.item.tileElement == (int)GameplayConstants.verticalArrow;
            hasBomb |= tile.item.tileElement == (int)GameplayConstants.bomb;
        }

        if (!hasArrow || !hasBomb)
        {
            return false;
        }

        FireHorizontalArrowBomb(touchedTiles);
        FireVerticalArrowBomb(touchedTiles);
        return true;
    }

    private void FireHorizontalArrowBomb(TileModel touchedTile)
    {
        for (int x = 0; x < model.Width; ++x)
        {
            DestroyTileAt(x, touchedTile.position.y - 1);
            DestroyTileAt(x, touchedTile.position.y);
            DestroyTileAt(x, touchedTile.position.y + 1);
        }
    }

    private void FireVerticalArrowBomb(TileModel touchedTile)
    {
        for (int y = 0; y < model.Height; ++y)
        {
            DestroyTileAt(touchedTile.position.x - 1, y);
            DestroyTileAt(touchedTile.position.x, y);
            DestroyTileAt(touchedTile.position.x + 1, y);
        }
    }

}
