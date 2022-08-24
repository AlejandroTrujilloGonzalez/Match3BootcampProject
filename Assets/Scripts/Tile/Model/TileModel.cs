using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileModel 
{
    public Vector2Int position;
    public TileItem item;

    public bool IsEmpty() => item == null;

}
