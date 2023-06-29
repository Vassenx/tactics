using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Tilemap baseTileMap;

    [SerializeField] private RuleTile clickTile;

    // temp
    [SerializeField] private Ally allyPrefab;
    private Ally curAllyClicked;
    
    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePos = baseTileMap.WorldToCell(worldPos);
            Vector3 centerOfTilePos = baseTileMap.GetCellCenterWorld(tilePos);
            centerOfTilePos.z += 0.5f;
        
            if(curAllyClicked != null)
                curAllyClicked.transform.position = centerOfTilePos;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider == null)
                return;

            if (hit.collider.CompareTag("Selector"))
            {
                if(BoardManager.Instance.clickTilePosDictionary.TryGetValue(hit.collider.gameObject, out Vector3Int pos))
                {
                    BoardManager.Instance.UpdateHighlightTileMap(new List<Cell>() { BoardManager.Instance.board[pos.x][pos.y] });
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider == null)
                return;

            if (hit.collider.CompareTag("Selector"))
            {
                if(BoardManager.Instance.clickTilePosDictionary.TryGetValue(hit.collider.gameObject, out Vector3Int pos))
                {
                    var cell = BoardManager.Instance.board[pos.x][pos.y];
                    if (cell.isObstructed)
                        return;
                    
                    if (cell.characterOnTile == null)
                    {
                        if (curAllyClicked != null)
                        {
                            List<Cell> path = new List<Cell>();
                            if (BoardManager.Instance.GetPath(curAllyClicked.curCellOn, cell, curAllyClicked.stats.movement, out path))
                            {
                                curAllyClicked.OnMove(path);
                                BoardManager.Instance.HideMovementTileOptions();
                            }
                        }
                    }
                    else
                    {
                        curAllyClicked = (Ally)cell.characterOnTile; // temp
                        BoardManager.Instance.ShowMovementTileOptions(curAllyClicked);
                    }
                }
            }
        }
        
        if (Input.GetMouseButtonDown(2))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider == null)
                return;

            if (hit.collider.CompareTag("Selector"))
            {
                if(BoardManager.Instance.clickTilePosDictionary.TryGetValue(hit.collider.gameObject, out Vector3Int pos))
                {
                    var cell = BoardManager.Instance.board[pos.x][pos.y];
                    if (cell.characterOnTile == null)
                    {
                        Vector3 centerOfTilePos = BoardManager.Instance.GetCellCenterWorld(cell);
                        centerOfTilePos.z += 1f;
                        var newAlly = Instantiate(allyPrefab, centerOfTilePos, allyPrefab.transform.rotation);
                        cell.characterOnTile = newAlly;
                        newAlly.curCellOn = cell;
                    }
                }
            }
        }
    }
}
