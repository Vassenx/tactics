using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/Stats")]
public class Stats : ScriptableObject
{
    public int movement = 1;
    public float movementSpeed = 4;
    public float maxHealth; // TODO: how to make private setter in a scriptable?
}
