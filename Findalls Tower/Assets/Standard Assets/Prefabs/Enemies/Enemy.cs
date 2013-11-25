using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    int health;
    int defense;
    int attack;
	private Transform currentTile;
	
	public Transform CurrentTile { get { return currentTile; } }

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
	
	private void TileVisibilityChanged (object sender)
	{
		Debug.Log("Tile Visibility Changed");
		if (currentTile.GetComponent<TileVisibility> ().IsVisible)
			MakeOpaque();
		else
			MakeTransparent ();
	}

    void OnCollisionEnter(Collision col)
    {	
		if (col.gameObject.CompareTag ("Path"))
		{
			if (col.gameObject.GetComponent<TileVisibility> ().IsVisible)
				MakeOpaque ();
			else
				MakeTransparent ();
			
			col.gameObject.GetComponent<TileVisibility> ().VisibilityChangedEvent += TileVisibilityChanged;
			
			if (currentTile != null)
				currentTile.GetComponent<TileVisibility> ().VisibilityChangedEvent -= TileVisibilityChanged;
			
			currentTile = col.transform;
		}
		
		//When colliding with player damage eachother.
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
	
	private void MakeTransparent ()
	{
		Debug.Log ("make transparent");
		Color thisColor = renderer.material.color;
		renderer.material.color = new Color (thisColor.r, thisColor.g, thisColor.b, 0f);
	}
	
	private void MakeOpaque ()
	{	
		Debug.Log ("make opaque");
		Color thisColor = renderer.material.color;
		renderer.material.color = new Color (thisColor.r, thisColor.g, thisColor.b, 1f);
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
