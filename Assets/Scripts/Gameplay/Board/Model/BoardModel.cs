using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardModel
{
    private TileModel[,] tiles;
    public int Width { get; }
    public int Height { get; }

    public BoardModel(int width, int height, TileItem[,] initialValues = null)
    {
        Width = width;
        Height = height;
        tiles = new TileModel[width, height];
        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                tiles[x, y] = new TileModel
                {
                    position = new Vector2Int(x, y),
                    item = initialValues?[x, y]
                };
            }
        }
    }

    public BoardModel(BoardModel other)
    {
        tiles = new TileModel[other.Width, other.Height];
        foreach (TileModel tile in other.tiles)
        {
            tiles[tile.position.x, tile.position.y] = new TileModel
            {
                position = tile.position,
                item = tile.item
            };
        }

        Width = other.Width;
        Height = other.Height;
    }

    public TileModel this[int x, int y]
    {
        get => tiles[x, y];
    }

    public TileModel this[Vector2Int pos]
    {
        get => tiles[pos.x, pos.y];
    }

    public void Clear()
    {
        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                tiles[x, y] = new TileModel
                {
                    position = new Vector2Int(x, y),
                    item = null
                };
            }
        }
    }

    public TileModel GetTile(int x, int y) => this[x, y];

    public TileModel GetTile(Vector2Int pos) => tiles[pos.x, pos.y];

}
