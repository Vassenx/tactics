using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// TileBase contains Tiles and RuleTiles
public class Cell
{
    public List<TileBase> verticalTiles { get; private set; } // includes topTile
    public TileBase topTile { get; private set; }
    public Vector3Int topTilePos { get; private set; }

    public Character characterOnTile;

    public Cell(List<TileBase> tiles, TileBase top, Vector3Int topPos)
    {
        verticalTiles = tiles;
        topTile = top;
        topTilePos = topPos;
    }
}