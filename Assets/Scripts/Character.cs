using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Shaders")]
    [SerializeField] private Material outlineMat;
    [SerializeField] private Material originalMat;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public Cell curCellOn;

    [Header("Character Info")]
    public Stats stats;
    public CharacterUIInfo UIInfo;
    
    public float health { get; protected set; }
    
    private List<Cell> path;

    private void Start()
    {
        path = new List<Cell>();
    }

    public void LateUpdate()
    {
        if (path.Count > 0)
        {
            MoveAlongPath();
        }
    }
    
    public virtual void Move(List<Cell> newPath)
    {
        path = newPath;
        
        spriteRenderer.material = originalMat;
    }

    private void MoveAlongPath()
    {
        curCellOn.characterOnTile = null;

        Cell nextCell = path[0];
        float height = nextCell.topTilePos.z + 1f;
        Vector2 nextCellPos = BoardManager.Instance.GetCellCenterWorld(nextCell);

        transform.position = Vector2.MoveTowards(transform.position, nextCellPos, stats.movementSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, height);

        if(Vector2.SqrMagnitude((Vector2)(transform.position) - nextCellPos) < 0.001f)
        {
            // slight offset to get sort order correct, TODO
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.0001f, transform.position.z + 1f);

            nextCell.characterOnTile = this;
            curCellOn = nextCell;
            
            path.Remove(nextCell);
        }
    }
    
    public void Attack(Character defender)
    {
        if (!ReferenceEquals(GetType(), defender.GetType())) // if enemy vs ally
        {
            defender.health = Mathf.Clamp(defender.health - 5f, 0f, defender.stats.maxHealth);
        }
    }

    public virtual void SelectCharacter()
    {
        spriteRenderer.material = outlineMat;
    }

    public virtual void DeselectCharacter()
    {
        spriteRenderer.material = originalMat;
    }
}
