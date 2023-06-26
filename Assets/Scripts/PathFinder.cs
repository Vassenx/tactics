using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// A* pathfinding
// via tutorial by Lawless Games: https://www.youtube.com/watch?v=u3hfWOCiIPg
public class PathFinder
{
    private List<Cell> openList;
    private List<Cell> closedList;
    
    public PathFinder()
    {
        openList = new List<Cell>();
        closedList = new List<Cell>();
    }

    public List<Cell> FindPath(Cell start, Cell end, int maxDistance)
    {
        ClearPathFinder();
        
        openList.Add(start);

        while (openList.Count > 0)
        {
            Cell curCell = openList.OrderBy(x => x.F).First();
            openList.Remove(curCell);
            closedList.Add(curCell);

            List<Cell> neighborCells = BoardManager.Instance.GetNeighborCells(curCell);

            foreach (Cell neighbor in neighborCells)
            {

                // 1 = max jump height
                if (closedList.Contains(neighbor) || neighbor.isObstructed ||
                    Mathf.Abs(neighbor.topTilePos.z - curCell.topTilePos.z) > 1)
                {
                    continue;
                }

                int gCost = GetManhattanDistance(start, neighbor);
                if (gCost > maxDistance)
                {
                    continue;
                }

                neighbor.G = gCost;
                neighbor.H = GetManhattanDistance(end, neighbor);
                neighbor.previousCell = curCell;
                
                if(!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
            }
        }

        return GetCalculatedPath(start, end);
    }

    // Grid-based distance func (how many x and y positions away)
    private int GetManhattanDistance(Cell c1, Cell c2)
    {
        return Mathf.Abs(c1.topTilePos.x - c2.topTilePos.x) + Mathf.Abs(c1.topTilePos.y - c2.topTilePos.y);
    }

    private List<Cell> GetCalculatedPath(Cell start, Cell end)
    {
        List<Cell> path = new List<Cell>();
        Cell curCell = end;

        while (curCell != start)
        {
            path.Add(curCell);
            curCell = curCell.previousCell;

            // TODO: if no path, return?
            if (curCell == null)
            {
                path.Clear();
                return path;
            }
        }

        path.Reverse();

        return path;
    }

    private void ClearPathFinder()
    {
        foreach (var cell in closedList)
        {
            cell.G = 0;
            cell.H = 0;
            cell.previousCell = null;
        }
        
        foreach (var cell in openList)
        {
            cell.G = 0;
            cell.H = 0;
            cell.previousCell = null;
        }
        
        openList.Clear();
        closedList.Clear();
    }
}
