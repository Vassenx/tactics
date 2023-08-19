using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Ally allyPrefab; // temp
    private Ally curAllyClicked;
    public Material originalMat;
    public Material outlineMat;

    public static Action<Character> OnSelectCharacter;

    private void Awake()
    {
        OnSelectCharacter += (character) =>
        {
            Ally allyChar = (Ally)character;
            if (allyChar != null)
            {
                SelectAlly(allyChar);
            }
        };
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cell cell = GetClickedCell();
            if (cell == null)
                return;
        
            if (cell.characterOnTile is Ally)
            {
                OnSelectCharacter?.Invoke(cell.characterOnTile);
            }
            else if (cell.characterOnTile is Enemy)
            {
                if (curAllyClicked != null)
                {
                    BoardManager.Instance.HideMovementCellOptions();
                    BoardManager.Instance.ShowSelectedCell(cell);
                    curAllyClicked.Attack(cell.characterOnTile);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
           Cell cell = GetClickedCell();
           
            if (cell == null || cell.isObstructed)
                return;

            if (cell.characterOnTile == null)
            {
                if (curAllyClicked != null)
                {
                    BoardManager.Instance.HideSelectedCell();
                    MoveAlly(cell);
                }
            }
        }
        
        // For debugging
        if (Input.GetMouseButtonDown(2))
        {
            Cell cell = GetClickedCell();
            
            if (cell == null || cell.isObstructed)
                return;
            
            BoardManager.Instance.HideMovementCellOptions();
            BoardManager.Instance.ShowSelectedCell(cell);
            
            BoardManager.Instance.SpawnCharacterAtCell(cell, allyPrefab);
            SelectAlly((Ally)cell.characterOnTile);
        }
    }

    private Cell GetClickedCell()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider == null)
            return null;

        if (hit.collider.CompareTag("Selector"))
        {
            if (BoardManager.Instance.clickTilePosDictionary.TryGetValue(hit.collider.gameObject,
                    out Vector3Int pos))
            {
                return BoardManager.Instance.board[pos.x][pos.y];
            }
        }

        return null;
    }

    public void SelectAlly(Ally ally)
    {
        if (curAllyClicked != null)
        {
            curAllyClicked.DeselectCharacter();
        }
        BoardManager.Instance.HideMovementCellOptions();

        BoardManager.Instance.ShowSelectedCell(ally.curCellOn);
        ally.SelectCharacter();
        
        curAllyClicked = ally;
    }

    private void MoveAlly(Cell cell)
    {
        List<Cell> path = new List<Cell>();
        if (BoardManager.Instance.GetPath(curAllyClicked.curCellOn, cell, curAllyClicked.stats.movement, out path))
        {
            curAllyClicked.Move(path);
            BoardManager.Instance.HideMovementCellOptions();
        }
    }
}
