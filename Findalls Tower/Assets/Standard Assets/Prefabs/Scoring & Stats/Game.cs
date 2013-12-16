using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
	private static int experience = 0;
	private static int points = 0;
	private static int dLevel = 1;
	private static int pLevel = 1;
	private int xpPerLevel = 2500;
	private static string xpScore = "";

	public static int DungeonLevel { get { return dLevel; } }
	// Use this for initialization
	void Start ()
	{
		UpdateXpScore ();
	}
	// Update is called once per frame
	void Update ()
	{
		if (experience >= xpPerLevel * pLevel)
			LevelUp ();
		UpdateXpScore ();
	}

	private void LevelUp ()
	{
		pLevel++;
		experience = 0;
		PlayerStats.LevelUp (pLevel);
	}

	private static void UpdateXpScore ()
	{
		xpScore = "Dungeon Level: " + dLevel + "\n\n";
		xpScore += "Score: " + points + "\n\n\n\n";

		xpScore += "Player level: " + pLevel + "\n\n";
		xpScore += "Experience: " + experience + "\n\n";
	}

	public static void EnemyKilled ()
	{
		experience += (dLevel + 1) * 100;
		points += (dLevel + 1) * 100;        
	}

	public static void ItemPickup ()
	{
		experience += dLevel * 100;
		points += dLevel * 100;
	}

	public static void TileUncovered ()
	{
		experience += (dLevel / 2) + 1;
		points += (dLevel / 2) + 1;
	}

	public static void NewLevel ()
	{
		dLevel++;
	}

	public static void Restart ()
	{
		dLevel = 1;
		pLevel = 1;
		points = 0;
		experience = 0;
	}

	void OnGUI ()
	{
		Rect guiPos = new Rect (Screen.width - 140, 0, Screen.width, Screen.height);
		GUI.Label (guiPos, xpScore);
	}
}
