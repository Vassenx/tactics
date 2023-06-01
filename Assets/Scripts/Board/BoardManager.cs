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

        HighlightAdjacentCells(cell, 0, character.stats.movement);

        foreach (Cell movementCell in movementCells)
        {
            var highlightMovementPos = movementCell.topTilePos;
            ++highlightMovementPos.z;
            highlightTileMap.SetTile(highlightMovementPos, highlightTile);
        }
    }

    // its recursive and not optimal. :p
    private void HighlightAdjacentCells(Cell baseCell, int stepIndex, int maxStepIndex)
    {
        Vector3Int cellPos = baseCell.topTilePos;

        if (stepIndex == maxStepIndex)
            return;
        
        if (board.Count > cellPos.x + 1)
        {
            var northCell = board[cellPos.x + 1][cellPos.y];
            if (!movementCells.Contains(northCell))
            {
                movementCells.Add(northCell);
            }
            HighlightAdjacentCells(northCell, stepIndex + 1, maxStepIndex);
        }

        if (board[cellPos.x].Count > cellPos.y + 1)
        {
            var westCell = board[cellPos.x][cellPos.y + 1];
            if (!movementCells.Contains(westCell))
            {
                movementCells.Add(westCell);
            }
            HighlightAdjacentCells(westCell, stepIndex + 1, maxStepIndex);
        }

        if (cellPos.x - 1 >= 0)
        {
            var southCell = board[cellPos.x - 1][cellPos.y];
            if (!movementCells.Contains(southCell))
            {
                movementCells.Add(southCell);
            }
            HighlightAdjacentCells(southCell, stepIndex + 1, maxStepIndex);
        }

        if (cellPos.y - 1 >= 0)
        {
            var eastCell = board[cellPos.x][cellPos.y - 1];
            if (!movementCells.Contains(eastCell))
            {
                movementCells.Add(eastCell);
            }
            HighlightAdjacentCells(eastCell, stepIndex + 1, maxStepIndex);
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
