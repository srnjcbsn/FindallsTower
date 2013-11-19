using UnityEngine;
using System.Collections;

public class Buff : Item 
{

	 
    //0: affectsCurrentHealth
    //1: affectsMaxHealth
    //2: affectsDefense
    //3: affectsAttack

    public Buff(int level, int offset, string descr)
    {
        description = descr;

        //Determine what stat is buffed
        int value = Random.Range(level, level + offset + 1);
        int stat = Random.Range(1, 3 + 1);

        Debug.Log("Buff stat: " + stat);
        
        statsAffected[stat] = value;

        //Small chance that a second (can be same) stat is buff
        if (Random.Range(0, 100) > 90)
        {
            value = Random.Range(level, level + offset+1);
            stat = Random.Range(1, 3+1);

            statsAffected[stat] += value;
        }
    }
}
