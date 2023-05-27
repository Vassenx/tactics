using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// TileBase contains Tiles and RuleTiles
public class Cell
{
    public List<GameObject> verticalTiles; // includes topTile
    public GameObject topTile;
    public Vector3Int topTilePos;

    public Cell(List<GameObject> tiles, GameObject top, Vector3Int topPos)
    {
        verticalTiles = tiles;
        topTile = top;
        topTilePos = topPos;
    }
}