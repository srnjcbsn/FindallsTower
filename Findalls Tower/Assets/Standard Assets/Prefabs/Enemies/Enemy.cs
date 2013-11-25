using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    int health;
    int defense;
    int attack;

    static int level;
    static int offset;

	// Use this for initialization
	void Start () 
    {
        level = Game.DungeonLevel;
        offset = level;

        health = Random.Range(level, level + offset + 1);
        defense = 0;
        attack = 1;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnCollisionEnter(Collision col)
    {
        //When colliding with player damage eachother.
        Debug.Log("Enemy collision");
        if (col.gameObject.name == "Player")
        {
            Debug.Log("Enemy collides with player");
            int playerAttack = PlayerStats.FightEnemy(attack);
            //Check to see if the enemy dies
            if (FightPlayer(playerAttack))
            {
                Game.EnemyKilled();
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
