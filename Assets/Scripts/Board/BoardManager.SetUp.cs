using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

// Initialization and editor functions
public partial class BoardManager : MonoBehaviour
{
    [Header("Tile Maps")]
    [SerializeField] public Tilemap baseTileMap;
    [SerializeField] private Tilemap overlayTileMap;
    [SerializeField] private Tilemap highlightTileMap;
    [SerializeField] private Tilemap clickTileMap;
    [SerializeField] private Tilemap obstructionTileMap;

    [Header("Special Tiles")]
    [SerializeField] private TileBase highlightTile;
    [SerializeField] private TileBase clickTile;

    [SerializeField] private List<TileBase> topSprites = new List<TileBase>();
    [SerializeField] private List<TileBase> overlaySprites = new List<TileBase>();
    private Dictionary<TileBase, TileBase> topToOverlaySprites;

    public Dictionary<GameObject, Vector3Int> clickTilePosDictionary;
    
    [ContextMenu("Move Board To Origin")]
    private void MoveBoardToOrigin()
    {
        // TODO: initialize board first? populate cells first + board = new ...
        Initialize();
        
        baseTileMap.ClearAllTiles();
        highlightTileMap.ClearAllTiles();
        
        for (int i = 0; i < board.Count; i++)
        {
            for (int j = 0; j < board[i].Count; j++)
            {
                var cell = board[i][j];
                for (int k = 0; k < cell.verticalTiles.Count; k++)
                {
                    var tile = cell.verticalTiles[k];
                    baseTileMap.SetTile(new Vector3Int(i,j, k), tile);
                }
            }
        }
        
        overlayTileMap.ClearAllTiles();
        UpdateOverlayTileMap();
        
        baseTileMap.RefreshAllTiles();
        overlayTileMap.RefreshAllTiles();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    private void PopulateBoard()
    {
        BoundsInt bounds = baseTileMap.cellBounds;
        int xPos = -1;
        board.Clear();

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            if (board.Count == 0 || board[xPos].Count > 0)
            {
                board.Add(new List<Cell>());
                ++xPos;
            }

            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                TileBase topTile = null;
                int heightZ = 0;
                List<TileBase> tiles = new List<TileBase>();
                
                for (int z = bounds.zMin; z < bounds.zMax; z++)
                {
                    var pos = new Vector3Int(x, y, z);
                    if (baseTileMap.HasTile(pos))
                    {
                        topTile = baseTileMap.GetTile<TileBase>(pos);
                        heightZ = z;
                        tiles.Add(topTile);
                    }
                    else
                    {
                        //break; // for optimization, but if theres no tile between two (height-wise), this will fail
                    }
                }

                if (board[0].Count == 0 && topTile == null)
                {
                    continue;
                }

                if (board[0].Count == 0 && topTile != null)
                {
                    Debug.LogWarning("please have first (bottom-most) tile at origin");
                }

                //highlightingTileMap.SetTile(new Vector3Int(x, y, heightZ+1), highlightTile);
                // TODO: add text

                if (tiles.Count > 0)
                {
                    var topTilePos = new Vector3Int(x, y, heightZ);
                    Cell cell = new Cell(tiles, topTile, topTilePos);
                    board[xPos].Add(cell);
                }
            }
        }

        // BoardManager.Instance.board = cells;
    }
    
    private void InitializeOverlayDictionary()
    {
        topToOverlaySprites = new Dictionary<TileBase, TileBase>();
        for (int i = 0; i < topSprites.Count; i++)
        {
            topToOverlaySprites.Add(topSprites[i], overlaySprites[i]);
        }
    }
    
    private void UpdateClickTileMap()
    {
        clickTileMap.ClearAllTiles();
        clickTilePosDictionary = new Dictionary<GameObject, Vector3Int>();
        
        for (int x = 0; x < board.Count; x++)
        {
            for (int y = 0; y < board[x].Count; y++)
            {
                Vector3Int topTilePos = board[x][y].topTilePos;
                ++topTilePos.z;
                clickTileMap.SetTile(topTilePos, clickTile);
                clickTilePosDictionary.Add(clickTileMap.GetInstantiatedObject(topTilePos), topTilePos);
            }
        }
    }
    
    private void UpdateObstructionTileMap()
    {
        obstructionTileMap.gameObject.SetActive(false);
        
        for (int x = 0; x < board.Count; x++)
        {
            for (int y = 0; y < board[x].Count; y++)
            {
                BoundsInt bounds = baseTileMap.cellBounds;

                for (int z = bounds.zMin; z < bounds.zMax; z++)
                {
                    var pos = new Vector3Int(x, y, z);
                    if(obstructionTileMap.HasTile(pos))
                    {
                        Cell obstructedCell = board[x][y];
                        obstructedCell.isObstructed = true;
                    }
                }
            }
        }
    }

    private void UpdateOverlayTileMap()
    {
        overlayTileMap.ClearAllTiles();
        
        for (int x = 0; x < board.Count; x++)
        {
            for (int y = 0; y < board[x].Count; y++)
            {
                Vector3Int topTilePos = board[x][y].topTilePos;
                TileBase topTile = board[x][y].topTile;
                
                if (topToOverlaySprites.TryGetValue(topTile, out TileBase overlayTile))
                {
                    var overlayTilePos = topTilePos;
                    ++overlayTilePos.z;
                    
                    overlayTileMap.SetTile(overlayTilePos, overlayTile);
                }
            }
        }
    }
}
