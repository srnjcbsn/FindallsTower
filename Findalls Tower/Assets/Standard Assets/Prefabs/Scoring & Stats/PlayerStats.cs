using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour 
{
    private static int maxHealth;
    public static int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

    private static int currentHealth;
    public static int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }

    private static int defense;
    public static int Armor { get { return defense; } set { defense = value; } }

    private static int baseAttack;
    public static int BaseAttack { get { return baseAttack; } set { baseAttack = value; } }

    private static int effectiveAttack;
    public static int EffectiveAttack { get { return effectiveAttack; } set { effectiveAttack = value; } }

    string stats;
    string health;
    string armor = "No armor";
    string weapon = "No weapon";
    string pickup = "Nothing";
    string debuff = "Nothing";

	// Use this for initialization
	void Start () 
    {
        UpdateStats();
	}
	
	// Update is called once per frame
	void Update () 
    {
        
        currentHealth = Player.CurrentHealth;
        maxHealth = Player.MaxHealth;
        defense = Player.Defense;
        effectiveAttack = Player.EffectiveAttack;
        armor = Player.Armor;
        weapon = Player.Weapon;
        pickup = Player.Pickup;
        debuff = Player.Debuff;
       

        if (currentHealth <= 0)
        {
            stats = "GAME OVER!";
        }
        else
	    {
            UpdateStats();
	    }

        
	}

    private void UpdateStats()
    {
        health = currentHealth + "/" + maxHealth;
        stats = "Health: " + health + "\n\n";
        stats += "Armor: " + defense + "\n\n";
        stats += "Attack: " + effectiveAttack + "\n\n";
        stats += "\n\n\n\n\n\n";
        stats += "Wearing: " + armor + "\n\n";
        stats += "Wielding: " + weapon + "\n\n";
        stats += "Affected by: " + pickup + "\n\n";
        stats += "Afflicted by: " + debuff + "\n\n";
    }

    void OnGUI()
    { 
        Rect guiPos = new Rect(0, 0, Screen.width, Screen.height);
        GUI.Label(guiPos, stats);
    }
}
