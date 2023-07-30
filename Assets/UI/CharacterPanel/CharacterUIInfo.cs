using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIInfo", menuName = "ScriptableObjects/UIInfo")]
public class CharacterUIInfo : ScriptableObject
{
    public string charName;
    public Sprite portrait;
    public Stats stats;
}