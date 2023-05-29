using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsController : MonoBehaviour
{
    [SerializeField] private GameObject unitMenuPrefab;
    [SerializeField] private GameObject actionMenuCanvas;
    private GameObject thisUnitMenu;
    // Start is called before the first frame update
    void Start()
    {
        //test
        InitializeActionMenu(unitMenuPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeActionMenu(GameObject actionMenuPrefab)
    {
        Vector3 positionToInstantiate = new Vector3(
            transform.position.x,
            transform.position.y - 0.5f,
            transform.position.z
        );
        thisUnitMenu = Instantiate(unitMenuPrefab,
            positionToInstantiate,
            new Quaternion(0,0,0,0),
            actionMenuCanvas.transform);
        
        //link unit with current menu

        if (thisUnitMenu.GetComponent<UnitMenuManager>() != null)
        {
            thisUnitMenu.GetComponent<UnitMenuManager>().AssociateUnitWithMenu(this.gameObject);
        }
    }
}
