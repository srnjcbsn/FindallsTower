using UnityEngine;
using System.Collections;

public class Item
{
    //0: affectsCurrentHealth
    //1: affectsMaxHealth
    //2: affectsDefense
    //3: affectsAttack

    public string description;

    protected int[] statsAffected = new int[4];

    public int[] StatsAffected { get { return statsAffected;} }
}
