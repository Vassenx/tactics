using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject player;
    [SerializeField] private Tilemap baseTileMap;

    [SerializeField] private RuleTile clickTile;

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
        
            player.transform.position = centerOfTilePos;
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
    }
}
