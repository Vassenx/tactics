using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Character
{
    public void OnMove(Cell nextCell)
    {
        curCell.characterOnTile = null;

        Vector3 curWorldPos = BoardManager.Instance.GetCellCenterWorld(curCell);
        Vector3 nextWorldPos = BoardManager.Instance.GetCellCenterWorld(nextCell);
        StartCoroutine(LerpPosition(curWorldPos, nextWorldPos, stats.movementSpeed));
        
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
