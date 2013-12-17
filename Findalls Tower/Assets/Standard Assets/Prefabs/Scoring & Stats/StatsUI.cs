using UnityEngine;

public class StatsUI : MonoBehaviour
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
	string lastPickedUp = "";
	// Use this for initialization
	void Start ()
	{
		UpdateStats ();
	}
	// Update is called once per frame
	void Update ()
	{
        
		currentHealth = PlayerStats.CurrentHealth;
		maxHealth = PlayerStats.MaxHealth;
		defense = PlayerStats.Defense;
		effectiveAttack = PlayerStats.Attack;
		armor = PlayerStats.Armor;
		weapon = PlayerStats.Weapon;
		pickup = PlayerStats.Pickup;
		debuff = PlayerStats.Debuff;
		lastPickedUp = PlayerStats.LastPickedUp;
       

		if (currentHealth <= 0)
		{
			stats = "GAME OVER!";
		} else
		{
			UpdateStats ();
		}
	}

	private void UpdateStats ()
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
		stats += "\n\nLast picked up:\n" + lastPickedUp + "\n\n";
	}

	void OnGUI ()
	{ 
		Rect guiPos = new Rect (0, 0, Screen.width, Screen.height);
		GUI.Label (guiPos, stats);
	}
}
