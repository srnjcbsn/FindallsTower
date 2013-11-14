using UnityEngine;
using System.Collections;

public class HealthGlobe : Item 
{
    
    //0: affectsCurrentHealth
    //1: affectsMaxHealth
    //2: affectsDefense
    //3: affectsAttack
    
    public HealthGlobe(int level, int offset)
    {
        description = "Health Globe";
        int r = Random.Range(level, level + offset);
        statsAffected[0] = r;
    }
}
