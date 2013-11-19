using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour 
{
    ItemType type;
    public Item item;

    public int level;
    public int offset;

	// Use this for initialization
	void Start () 
    {
        level = 1;
        offset = 2;

        //Determine the kind of item the object instance is representing
        int r = Random.Range(0, sizeof(ItemType));
        r = 2; //TESTING
        switch (r)
        {
            case 0:
                this.type = ItemType.Armor;
                break;
            case 1:
                this.type = ItemType.Debuff;
                break;
            case 2:
                this.type = ItemType.Buff;
                item = new Buff(level, offset-1, "Buff");
                break;
            case 3:
                this.type = ItemType.HealthGlobe;
                item = new HealthGlobe(level, offset, "Health Globe");
                break;
            case 4:
                this.type = ItemType.Weapon;
                break;
            default:
                this.type = ItemType.HealthGlobe;
                break;

        }
    }

	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player")
        {
            Player.ItemPickup(item);
            gameObject.SetActive(false);
        }

    }
}
