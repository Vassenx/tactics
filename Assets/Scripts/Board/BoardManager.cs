using System.Collections.Generic;
using UnityEngine;

public partial class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }
    
    public List<List<Cell>> board { get; private set; }
    public List<Cell> movementCells { get; private set; }

    private PathFinder pathFinder;
    
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

        PlaceCharactersOnBoard();
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
        Cell cell = character.curCellOn;

        HighlightAdjacentCells(cell, 0, character.stats.movement);

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
            var highlightMovementPos = movementCell.topTilePos;
            ++highlightMovementPos.z;
            highlightTileMap.SetTile(highlightMovementPos, null);
        }
        
        movementCells.Clear();
    }

    // its recursive and not optimal. :p
    private void HighlightAdjacentCells(Cell baseCell, int stepIndex, int maxStepIndex)
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
            
            HighlightAdjacentCells(neighbor, stepIndex + 1, maxStepIndex);
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
