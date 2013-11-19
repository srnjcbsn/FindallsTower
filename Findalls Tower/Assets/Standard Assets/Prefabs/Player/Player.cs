using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private static int maxHealth;
    public static int MaxHealth { get { return maxHealth; } set { maxHealth = value;} }

    private static int currentHealth;
    public static int CurrentHealth { get { return currentHealth; } set { currentHealth = value;}  }

    private static int defense;
    public static int Defense { get { return defense; } set { defense = value; } }

    private static int baseAttack;
    public static int BaseAttack { get { return baseAttack; } set { baseAttack = value; } }

    private static int effectiveAttack;
    public static int EffectiveAttack { get { return effectiveAttack; } set { effectiveAttack = value; } }

    private static Item armor;
    public static string Armor
    {
        get
        {
            if (armor != null)
                return armor.description;
            else
                return "No armor";
        }
    }

    private static Item weapon;
    public static string Weapon
    {
        get
        {
            if (weapon != null)
                return weapon.description;
            else
                return "No weapon";
        }
    }

    private static Item pickup;
    public static string Pickup
    {
        get
        {
            if (pickup != null)
                return pickup.description;
            else
                return "Nothing";
        }
    }


	// Use this for initialization
	void Start () 
    {
        maxHealth = 4;
        currentHealth = maxHealth;
        defense = 0;
        baseAttack = 1;
        effectiveAttack = baseAttack;
    }
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    //Handles the different items the player can pick up
    public static void ItemPickup(Item item)
    {
        //0: affectsCurrentHealth
        //1: affectsMaxHealth
        //2: affectsDefense
        //3: affectsAttack

        Debug.Log("Item pickup");
        Debug.Log("Value healed: " + item.StatsAffected[0]);

        //Heals player - one time effect
        if (item.GetType() == typeof(HealthGlobe))
        {
            CurrentHealth += item.StatsAffected[0];
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
            
            Debug.Log("Player health: " + CurrentHealth);
            return;
        }
        //Buffs/debuffs player - effect is equipped and stays on
        else if (item.GetType() == typeof(Buff))
        {
            if (pickup != null)
            {
                UnEquip(pickup);
                pickup = null;
            }
            pickup = item;
            Equip(item);
        }
    }

    //Applies damage from an enemy and returns the players current attack to damage the enemy
    public static int FightEnemy(int enemyAttack)
    {
        currentHealth -= (enemyAttack - defense);

        if (currentHealth < 0)
            Dead();

        return effectiveAttack;
    }

    //Adds or subtracts values from the stat(s) affected by a current item
    private static void Equip(Item item)
    {
        //0: affectsCurrentHealth
        //1: affectsMaxHealth
        //2: affectsDefense
        //3: affectsAttack

        for (int i = 0; i < item.StatsAffected.Length; i++)
        {
            if (item.StatsAffected[i] != 0)
            {
                switch (i)
                { 
                    case 0:
                        break;
                    case 1:
                        maxHealth += item.StatsAffected[i];
                        if (maxHealth < 1)
                            maxHealth = 1;
                        if (currentHealth > maxHealth)
                            currentHealth = maxHealth;
                        break;
                    case 2:
                        defense += item.StatsAffected[i];
                        break;
                    case 3:
                        effectiveAttack += item.StatsAffected[i];
                        if (effectiveAttack < 1)
                            effectiveAttack = 1;
                        break;
                    default:
                        break;

                }
            }
        }
    }

    //The reverse of equip
    private static void UnEquip(Item item)
    {
        //0: affectsCurrentHealth
        //1: affectsMaxHealth
        //2: affectsDefense
        //3: affectsAttack

        for (int i = 0; i < item.StatsAffected.Length; i++)
        {
            if (item.StatsAffected[i] != 0)
            {
                switch (i)
                {
                    case 0:
                        break;
                    case 1:
                        maxHealth -= item.StatsAffected[i];
                        break;
                    case 2:
                        defense -= item.StatsAffected[i];
                        break;
                    case 3:
                        effectiveAttack -= item.StatsAffected[i];
                        break;
                    default:
                        break;

                }
            }
        }
    }

    private static void Dead()
    {
        Debug.Log("DEAD!!!");
    }

}
