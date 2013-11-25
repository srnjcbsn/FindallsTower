using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour 
{
    ItemType type;
    public Item item;

    private static int level = Game.DungeonLevel;
    private static int offset = level + 1;

	// Use this for initialization
	void Start () 
    {
        

        //Determine the kind of item the object instance is representing
        int r = Random.Range(0, sizeof(ItemType));

        //0: affectsCurrentHealth
        //1: affectsMaxHealth
        //2: affectsDefense
        //3: affectsAttack
        switch (r)
        {
            case 0:
                this.type = ItemType.Armor;
                item = new Item(level, offset - 1, "Armor", 2, ItemType.Armor);
                break;
            case 1:
                this.type = ItemType.Debuff;
                item = new Item(level, offset - 1, ItemType.Debuff);
                break;
            case 2:
                this.type = ItemType.Buff;
                item = new Item(level, offset - 1, "Health Buff", 1, ItemType.Buff);
                break;
            case 3:
                this.type = ItemType.HealthGlobe;
                item = new Item(level, offset, "Health Globe", 0, ItemType.HealthGlobe);
                break;
            case 4:
                this.type = ItemType.Weapon;
                item = new Item(level, offset - 1, "Weapon", 3, ItemType.Weapon);
                break;
            default:
                this.type = ItemType.HealthGlobe;
                item = new Item(level, offset, "Health Globe", 0, ItemType.HealthGlobe);
                break;

        }
    }

	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Colission");
        if (col.gameObject.name == "Player")
        {
            Game.ItemPickup();
            PlayerStats.ItemPickup(item);
            gameObject.SetActive(false);
        }

    }
}
