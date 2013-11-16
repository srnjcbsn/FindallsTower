﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    int health;
    int defense;
    int attack;

    int level;
    int offset;

	// Use this for initialization
	void Start () 
    {
        level = 1;
        offset = 1;

        health = Random.Range(level, level + offset);
        defense = 0;
        attack = 1;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Enemy collision");
        if (col.gameObject.name == "Player")
        {
            Debug.Log("Enemy collides with player");
            int playerAttack = Player.FightEnemy(attack);
            if (FightPlayer(playerAttack))
            {
                Dead();
            }
            
        }

    }

    bool FightPlayer(int playerAttack)
    {
        health -= (playerAttack - defense);

        return health <= 0;
    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }
}