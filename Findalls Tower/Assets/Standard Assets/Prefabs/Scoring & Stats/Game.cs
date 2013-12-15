﻿using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour 
{

    private static int experience = 0;
    private static int points = 0;

    private static int dLevel = 1;
    private static int pLevel = 1;

    private int xpPerLevel = 20;


    private static string xpScore = "";

    public static int DungeonLevel { get { return dLevel; } }

	// Use this for initialization
	void Start () 
    {
        UpdateXpScore();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (experience >= xpPerLevel * pLevel)
            LevelUp();
        UpdateXpScore();
	}

    private void LevelUp()
    {
        pLevel++;
        experience = 0;
        PlayerStats.LevelUp(pLevel);
    }

    private static void UpdateXpScore()
    {
        xpScore = "Dungeon Level: " + dLevel + "\n\n";
        xpScore += "Score: " + points + "\n\n\n\n";

        xpScore += "Player level: " + pLevel + "\n\n";
        xpScore += "Experience: " + experience + "\n\n";
    }

    public static void EnemyKilled()
    {
        experience += dLevel + 1;
        points += dLevel + 1;        
    }

    public static void ItemPickup()
    {
        experience += dLevel;
        points += dLevel;
    }

    public static void NewLevel()
    {
        dLevel++;
    }

    void OnGUI()
    {
        Rect guiPos = new Rect(Screen.width-140, 0, Screen.width, Screen.height);
        GUI.Label(guiPos, xpScore);
    }
}
