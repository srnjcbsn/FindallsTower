using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

    private static int maxHealth;
    public static int MaxHealth { get { return maxHealth; } set { maxHealth = value;} }

    private static int currentHealth;
    public static int CurrentHealth { get { return currentHealth; } set { currentHealth = value;}  }

    private static int defense;
    public static int Defense { get { return defense; } set { defense = value; } }

    private static int attack;
    public static int Attack { get { return attack; } set { attack = value; } }
	
	public int visionRange;

    private static string lastPickedUp = "";
    public static string LastPickedUp { get { return lastPickedUp;} }

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

    private static Item debuff;
    public static string Debuff
    {
        get
        {
            if (debuff != null)
                return debuff.description;
            else
                return "Nothing";
        }
    }


	// Use this for initialization
	void Start () 
    {
		InitializePlayer ();
    }

	public static void InitializePlayer ()
	{
		armor = null;
		debuff = null;
		weapon = null;
		pickup = null;

		maxHealth = 4;
		currentHealth = maxHealth;
		defense = 0;        
		attack = 1;
		lastPickedUp = "";
	}

    //Handles the different items the player can pick up
    public static void ItemPickup(Item item)
    {
        //0: affectsCurrentHealth
        //1: affectsMaxHealth
        //2: affectsDefense
        //3: affectsAttack

        lastPickedUp = item.description;


        switch (item.type)
        {
            case ItemType.HealthGlobe:
                
                if (debuff != null)
                {
                    UnEquip(debuff);
                    debuff = null;
                }
                CurrentHealth += item.StatsAffected[0];
                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;
                break;
            case ItemType.Buff:
                if (pickup != null)
                {
                    UnEquip(pickup);
                    pickup = null;
                }
                pickup = item;
                Equip(item);
                break;
            case ItemType.Armor:
                if (armor != null)
                {
                    UnEquip(armor);
                    armor = null;
                }
                armor = item;
                Equip(item);
                break;
            case ItemType.Weapon:
                if (weapon != null)
                {
                    UnEquip(weapon);
                    weapon = null;
                }
                weapon = item;
                Equip(item);
                break;
            case ItemType.Debuff:
                if (debuff != null)
                {
                    UnEquip(debuff);
                    debuff = null;
                }
                debuff = item;
                Equip(item);
                break;
            default:
                break;
        }  
    }

    //Applies damage from an enemy and returns the players current attack to damage the enemy
    public static int FightEnemy(int enemyAttack)
    {
        if (enemyAttack - defense > 0)
            currentHealth -= (enemyAttack - defense);

		if (currentHealth <= 0)
            Dead();

        return attack;
    }

    //Adds or subtracts values from the stat(s) affected by a current item
    private static void Equip(Item item)
    {
        //0: affectsCurrentHealth
        //1: affectsMaxHealth
        //2: affectsDefense
        //3: affectsAttack

        for (int i = 1; i < item.StatsAffected.Length; i++)
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
                        attack += item.StatsAffected[i];
                        if (attack < 1)
                            attack = 1;
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
                        attack -= item.StatsAffected[i];
                        break;
                    default:
                        break;

                }
            }
        }
    }

    public static void LevelUp(int level)
    {
        level--;
        maxHealth++;
        currentHealth = maxHealth;
        attack += (int)System.Math.Floor(level/2.0);
        defense += (int)System.Math.Floor(level / 3.0);
    }

    private static void Dead()
    {
		GameObject.FindWithTag ("Plane").GetComponent<PlaneScript> ().Restart ();
    }
}
