using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Character
{
    public Cell curCell;

    public void OnMove(Cell nextCell)
    {
        curCell.characterOnTile = null;

        Vector3 curWorldPos = BoardManager.Instance.baseTileMap.CellToWorld(curCell.topTilePos);
        Vector3 nextWorldPos = BoardManager.Instance.baseTileMap.CellToWorld(nextCell.topTilePos);
        StartCoroutine(LerpPosition(curWorldPos, nextWorldPos, 5));
        
        nextCell.characterOnTile = this;
        curCell = nextCell;
    }

    IEnumerator LerpPosition(Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
