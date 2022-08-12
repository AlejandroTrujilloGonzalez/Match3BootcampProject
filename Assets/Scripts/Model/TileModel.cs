using UnityEngine;
public class TileModel
{
    public Vector2Int tileLocation;
    public TileType type;

    public TileModel(Vector2Int tileLocation, TileType type)
    {
        this.tileLocation = tileLocation;
        this.type = type;
    }
}

public enum TileType
{
    blue, green, orange, red, violet
}