using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

// Initialization and editor functions
public partial class BoardManager : MonoBehaviour
{
    [Header("Tile Maps")]
    [SerializeField] private List<Tilemap> baseTileMaps; // 3D needs 1 map per height
    [SerializeField] private Tilemap overlayTileMap;
    [SerializeField] private Tilemap highlightTileMap;
    
    [Header("Special Tiles")]
    [SerializeField] private GameObject highlightTile;
    
    [SerializeField] private List<GameObject> topSprites = new List<GameObject>();
    [SerializeField] private List<GameObject> overlaySprites = new List<GameObject>();
    private Dictionary<GameObject, GameObject> topToOverlaySprites;

    private void PopulateBoard()
    {

        PopulateBoardMap();
        
        for (int i = 1; i < baseTileMaps.Count; i++)
        {
            PopulateHeights(i);
        }
    }

    // expensive, please dont use at runtime
    private void PopulateBoardMap()
    {
        board.Add(new List<Cell>());

        List<Transform> children =  new List<Transform>(baseTileMaps[0].GetComponentsInChildren<Transform>());
        children.Remove(baseTileMaps[0].transform); // ignore parent
        List<Transform> childrenList = children.OrderBy(c => c.position.z).ThenBy(c => c.position.x).ToList();

        Transform prevCube = null;
        int x = 0;
        for (int y = 0; y < childrenList.Count; y++)
        {
            Transform cube = childrenList[y];
            if (prevCube != null && prevCube.position.z < cube.position.z)
            {
                ++x;
                
                board.Add(new List<Cell>());
            }

            Cell cell = new Cell(new List<GameObject>() {cube.gameObject},cube.gameObject, new Vector3Int(x,y,0));
            Instance.board[x].Add(cell);
            
            prevCube = cube;
        }
    }

    private void PopulateHeights(int mapIndex)
    {
        List<Cell> boardFlatten = board.SelectMany(x => x).ToList();

        List<Transform> children =  new List<Transform>(baseTileMaps[mapIndex].GetComponentsInChildren<Transform>());
        children.Remove(baseTileMaps[mapIndex].transform);

        foreach (var cube in children)
        {
            Cell cell = boardFlatten.FirstOrDefault(boardCell => 
                Mathf.Approximately(boardCell.topTile.transform.position.z, cube.position.z) &&
                Mathf.Approximately(boardCell.topTile.transform.position.x,cube.position.x));

            if (cell == null)
            {
                Debug.LogError("no bottom cell");
            }
            cell.topTile = cube.gameObject; 
            cell.verticalTiles.Add(cube.gameObject);
            cell.topTilePos.z++;
        }
    }
    
    private void InitializeOverlayDictionary()
    {
        topToOverlaySprites = new Dictionary<GameObject, GameObject>();
        for (int i = 0; i < topSprites.Count; i++)
        {
            topToOverlaySprites.Add(topSprites[i], overlaySprites[i]);
        }
    }
}
