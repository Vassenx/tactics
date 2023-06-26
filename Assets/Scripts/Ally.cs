using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Character
{
    private List<Cell> path;

    private void Start()
    {
        path = new List<Cell>();
    }

    public void OnMove(List<Cell> newPath)
    {
        path = newPath;
    }

    public void LateUpdate()
    {
        if (path.Count > 0)
        {
            curCellOn.characterOnTile = null;

            Cell nextCell = path[0];
            int height = nextCell.topTilePos.z;
            Vector2 nextCellPos = BoardManager.Instance.GetCellCenterWorld(nextCell);

            transform.position = Vector2.MoveTowards(transform.position, nextCellPos, stats.movementSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, height);

            if(Vector2.SqrMagnitude((Vector2)(transform.position) - nextCellPos) < 0.001f)
            {
                // slight offset to get sort order correct, TODO
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.0001f, transform.position.z);

                curCellOn = nextCell;
                path.Remove(nextCell);
            }
        }
    }
}
