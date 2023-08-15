using System;
using System.Collections.Generic;
using UnityEngine;

public partial class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }
    
    public List<List<Cell>> board { get; private set; }
    public List<Cell> movementCells { get; private set; }
    public Cell curSelectedCell { get; private set; }

    private PathFinder pathFinder;

    public static Action OnBoardInitialized;
    
    private void Awake()
    {
        Initialize();

        pathFinder = new PathFinder();
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
        InitializeCharacterLocationDictionary();
        UpdateOverlayTileMap();
        UpdateObstructionTileMap();
        UpdateClickTileMap();
        UpdateCharacterPlacementTileMap();
        
        OnBoardInitialized?.Invoke();
    }

    public Vector3 GetCellCenterWorld(Cell cell)
    {
        Vector3 centerOfTilePos = baseTileMap.GetCellCenterWorld(cell.topTilePos);
        centerOfTilePos.y += 0.25f;

        return centerOfTilePos;
    }
    
    public void ShowSelectedCell(Cell newSelectedCell)
    {
        HideSelectedCell();
        
        var newTopPos = newSelectedCell.topTilePos;
        ++newTopPos.z;
        highlightTileMap.SetTile(newTopPos, selectedTile);
        
        curSelectedCell = newSelectedCell;
    }

    public void HideSelectedCell()
    {
        if (curSelectedCell != null)
        {
            var curTopPos = curSelectedCell.topTilePos;
            ++curTopPos.z;
            highlightTileMap.SetTile(curTopPos, null);
        }
    }

    public void ShowMovementCellOptions(Character character)
    {
        Cell cell = character.curCellOn;

        GetMovementCells(cell, 0, character.stats.movement);
        movementCells.Remove(cell); // don't include cell the character is already on
        
        foreach (Cell movementCell in movementCells)
        {
            var highlightMovementPos = movementCell.topTilePos;
            ++highlightMovementPos.z;
            highlightTileMap.SetTile(highlightMovementPos, highlightTile);
        }
    }
    
    public void HideMovementCellOptions()
    {
        foreach (Cell movementCell in movementCells)
        {
            var highlightMovementPos = movementCell.topTilePos;
            ++highlightMovementPos.z;
            highlightTileMap.SetTile(highlightMovementPos, null);
        }
        
        movementCells.Clear();
    }

    // its recursive and not optimal. :p
    private void GetMovementCells(Cell baseCell, int stepIndex, int maxStepIndex)
    {
        if (stepIndex == maxStepIndex)
            return;

        var neighborCells = GetNeighborCells(baseCell);
        foreach (Cell neighbor in neighborCells)
        {
            if (neighbor.isObstructed) // TODO: indication that its unselectable
                return;
        
            if (!movementCells.Contains(neighbor))
            {
                movementCells.Add(neighbor);
            }
            
            GetMovementCells(neighbor, stepIndex + 1, maxStepIndex);
        }
    }

    public List<Cell> GetNeighborCells(Cell cell)
    {
        var neighbors = new List<Cell>();
        
        Vector3Int cellPos = cell.topTilePos;

        if (board.Count > cellPos.x + 1)
        {
            var northCell = board[cellPos.x + 1][cellPos.y];
            neighbors.Add(northCell);
        }

        if (board[cellPos.x].Count > cellPos.y + 1)
        {
            var westCell = board[cellPos.x][cellPos.y + 1];
            neighbors.Add(westCell);
        }

        if (cellPos.x - 1 >= 0)
        {
            var southCell = board[cellPos.x - 1][cellPos.y];
            neighbors.Add(southCell);
        }

        if (cellPos.y - 1 >= 0)
        {
            var eastCell = board[cellPos.x][cellPos.y - 1];
            neighbors.Add(eastCell);
        }

        return neighbors;
    }

    public bool GetPath(Cell start, Cell end, int maxDistance, out List<Cell> path)
    {
        path = pathFinder.FindPath(start, end, maxDistance);
        return path.Count > 0;
    }
}
