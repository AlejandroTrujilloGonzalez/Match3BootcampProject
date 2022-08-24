using UnityEngine;
public class Tile
{
    public Vector2Int tileLocation;
    public TileType type;

    public Tile(Vector2Int tileLocation, TileType type)
    {
        this.tileLocation = tileLocation;
        this.type = type;
    }
}

public enum TileType
{
    blue, green, orange, red, violet
}