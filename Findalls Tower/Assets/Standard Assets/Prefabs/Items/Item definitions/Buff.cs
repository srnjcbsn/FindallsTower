using UnityEngine;
using System.Collections;

public class Buff : Item 
{

	 
    //0: affectsCurrentHealth
    //1: affectsMaxHealth
    //2: affectsDefense
    //3: affectsAttack

    public Buff(int level, int offset)
    {
        description = "Buff";
        int value = Random.Range(level, level + offset);
        int stat = Random.Range(1, 3);
        
        statsAffected[stat] = value;

        if (Random.Range(0, 100) > 90)
        {
            value = Random.Range(level, level + offset);
            stat = Random.Range(1, 3);

            statsAffected[stat] += value;
        }
    }
}
