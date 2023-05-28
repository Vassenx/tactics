using System.Collections.Generic;
using UnityEngine;

public partial class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }
    
    public List<List<Cell>> board { get; private set; }
    
    private void Awake()
    {
        Initialize();
    }

    [ContextMenu("Initialize Board")]
    public void Initialize()
    {
        
#region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
#endregion

        board = new List<List<Cell>>();
        PopulateBoard();
        // MoveBoardToOrigin();
        InitializeOverlayDictionary();
        UpdateOverlayTileMap();
        UpdateClickTileMap();
    }
    
    // TODO clear old highlighting tiles
    public void UpdateHighlightTileMap(List<Cell> cellsToHighlight)
    {
        foreach (var cell in cellsToHighlight)
        {
            var topPos = cell.topTilePos;
            ++topPos.z;
            highlightTileMap.SetTile(topPos, highlightTile);
        }
    }
}
