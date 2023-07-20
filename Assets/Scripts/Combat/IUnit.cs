using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUnit
{
  string name;
  string[] dialogue;
  int health;
  // int mana;
  int range;
  int speed; // e.g. chance of dodging, chance of disengage, distance can move per turn
  int attackPower;
  int defensePower;
  int[] position;
  // Item[] inventory;
  // Ability[] abilities;
  public abstract void attack();
  public abstract void useItem();
  public abstract void die();
}
