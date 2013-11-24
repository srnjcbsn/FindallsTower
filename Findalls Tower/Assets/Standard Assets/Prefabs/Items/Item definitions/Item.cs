using UnityEngine;
using System.Collections;

public class Item
{
    //0: affectsCurrentHealth
    //1: affectsMaxHealth
    //2: affectsDefense
    //3: affectsAttack

    public string description;

    public ItemType type;

    protected int[] statsAffected = new int[sizeof(ItemType)-1];

    public int[] StatsAffected { get { return statsAffected;} }

   
    public Item(int level, int offset, string descr, int stat, ItemType type)
    {
        this.type = type;
        description = descr;
        //Determine buff range
        int value = Random.Range(level, level + offset + 1);
               
        statsAffected[stat] = value;

        //Small chance of double buff
        if (Random.Range(0, 100) > 99)
        {
            value = Random.Range(level, level + offset + 1);
            
            statsAffected[stat] += value;
        }
    }

    //Debuff
    public Item(int level, int offset, ItemType type)
    {
        this.type = type;
        //Determine what stat is debuffed
        int stat = Random.Range(1, statsAffected.Length);

        //Describe debuff
        switch (stat)
        {
            case 1:
                description = "Health Debuff";
                break;
            case 2:
                description = "Defense Debuff";
                break;
            case 3:
                description = "Attack Debuff";
                break;
            default:
                description = "Debuff";
                break;
        }

        //Determine value of debuff
        int value = Random.Range(level, level + offset + 1) * -1;

        statsAffected[stat] = value;

        //Small chance of double buff
        if (Random.Range(0, 100) > 99)
        {
            value = Random.Range(level, level + offset + 1) * -1;

            statsAffected[stat] += value;
        }

    }
}
