using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject player;
    [SerializeField] private Tilemap baseTileMap;

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
    }
    
    private void OnMouseDown()
    {
        var board = BoardManager.Instance.board;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var curPos = baseTileMap.WorldToCell(mouseWorldPos);
        var cell = board[curPos.x][curPos.y];
        BoardManager.Instance.UpdateHighlightTileMap(new List<Cell>() {cell});
    }
}
