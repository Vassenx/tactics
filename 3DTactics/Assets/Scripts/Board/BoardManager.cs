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
        InitializeOverlayDictionary();
    }
    
    // TODO
    public void SetCell(Vector3Int pos, GameObject newCube)
    {
        // TODO should i set anything in the tilemaps?
        if(baseTileMaps.Count <= pos.z)
            Debug.LogError("no");
        
        if (board.Count <= pos.x || board[pos.x].Count <= pos.y)
        {
            Debug.LogError("TODO");

        }
        var tileMap = baseTileMaps[pos.z];
        var oldCube = Instance.board[pos.x][pos.y].verticalTiles[pos.z];

        newCube.transform.SetParent(tileMap.transform);
        newCube.transform.position = oldCube.transform.position;
        Destroy(oldCube);
    }
}
