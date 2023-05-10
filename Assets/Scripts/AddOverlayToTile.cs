using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AddOverlayToTile : MonoBehaviour
{
    [SerializeField] private Tile testTile;
    [SerializeField] private List<Tile> topSprites = new List<Tile>();
    [SerializeField] private List<Tile> overlaySprites = new List<Tile>();
    private Dictionary<Tile, Tile> topToOverlaySprites;

    [SerializeField] private Tilemap tileMap;

    public void UpdateDictionary()
    {
        topToOverlaySprites = new Dictionary<Tile, Tile>();
        for (int i = 0; i < topSprites.Count; i++)
        {
            topToOverlaySprites.Add(topSprites[i], overlaySprites[i]);
        }
    }

    [ContextMenu("Update TileMap")]
    void UpdateTileMap()
    {
        UpdateDictionary();
        
        BoundsInt bounds = tileMap.cellBounds; 
        
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Tile highestTile = null;
                int heightZ = 0;
                
                for (int z = bounds.zMin; z < bounds.zMax; z++)
                {
                    var pos = new Vector3Int(x, y, z);
                    if (tileMap.HasTile(pos))
                    {
                        highestTile = tileMap.GetTile<Tile>(pos);
                        heightZ = z;
                    }
                    else
                    {
                        //break; // for optimization, but if theres no tile between two (height-wise), this will fail
                    }
                }

                Tile overlayTile;
                if (highestTile != null && topToOverlaySprites.TryGetValue(highestTile, out overlayTile))
                {
                    tileMap.SetTile(new Vector3Int(x, y, heightZ + 1), overlayTile);
                }
            }
        }        
    }
}
