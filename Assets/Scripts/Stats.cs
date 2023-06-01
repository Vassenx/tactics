using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/Stats")]
public class Stats : ScriptableObject
{
    public int movement = 1;
    public float movementSpeed = 4;
}
