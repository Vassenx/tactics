using System.Collections.Generic;
using UnityEngine;

public partial class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }
    
    public List<List<Cell>> board { get; private set; }
    public List<Cell> movementCells { get; private set; }
    
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
        movementCells = new List<Cell>();

        PopulateBoard();
        // MoveBoardToOrigin();
        InitializeOverlayDictionary();
        UpdateOverlayTileMap();
        UpdateClickTileMap();
    }

    public Vector3 GetCellCenterWorld(Cell cell)
    {
        Vector3 centerOfTilePos = baseTileMap.GetCellCenterWorld(cell.topTilePos);
        centerOfTilePos.y += 0.25f;

        return centerOfTilePos;
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

    public void ShowMovementTileOptions(Character character)
    {
        Cell cell = character.curCell;
        Vector3Int cellPos = cell.topTilePos;
        int movementAmount = character.stats.movement;

        for (int i = 1; i <= movementAmount; i++)
        {
            if (board.Count > cellPos.x + i)
            {
                var northCell = board[cellPos.x + i][cellPos.y];
                movementCells.Add(northCell);
            }

            if (board[cellPos.x].Count > cellPos.y + i)
            {
                var westCell = board[cellPos.x][cellPos.y + i];
                movementCells.Add(westCell);
            }

            if (cellPos.x - i >= 0)
            {
                var southCell = board[cellPos.x - i][cellPos.y];
                movementCells.Add(southCell);
            }

            if (cellPos.y - i >= 0)
            {
                var eastCell = board[cellPos.x][cellPos.y - i];
                movementCells.Add(eastCell);
            }

            // diagonals = 2 movements
            if (i > 1)
            {
                int diag = i - 1;
                if (board.Count > cellPos.x + diag && board[cellPos.x].Count > cellPos.y + diag)
                {
                    var northwestCell = board[cellPos.x + diag][cellPos.y + diag];
                    movementCells.Add(northwestCell);
                }
                
                if (board.Count > cellPos.x + diag && cellPos.y - diag >= 0)
                {
                    var northeastCell = board[cellPos.x + diag][cellPos.y - diag];
                    movementCells.Add(northeastCell);
                }
                
                if (board[cellPos.x].Count > cellPos.y + diag && board[cellPos.x].Count > cellPos.y + diag)
                {
                    var southwestCell = board[cellPos.x - diag][cellPos.y + diag];
                    movementCells.Add(southwestCell);
                }
                
                if (board[cellPos.x].Count > cellPos.y + diag && cellPos.x - diag >= 0)
                {
                    var southeastCell = board[cellPos.x - diag][cellPos.y - diag];
                    movementCells.Add(southeastCell);
                }
            }
        }

        foreach (Cell movementCell in movementCells)
        {
            var highlightMovementPos = movementCell.topTilePos;
            ++highlightMovementPos.z;
            highlightTileMap.SetTile(highlightMovementPos, highlightTile);
        }
    }

    public void HideMovementTileOptions()
    {
        foreach (Cell movementCell in movementCells)
        {
            highlightTileMap.SetTile(movementCell.topTilePos, null);
        }
    }
}
