using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject assignedUnit;

    public void AssociateUnitWithMenu(GameObject linkedUnit)
    {
        assignedUnit = linkedUnit;
    }

    public GameObject GetAssignedUnit()
    {
        return this.assignedUnit;
    }
}
