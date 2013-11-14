using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public float Force = 10;
    public int ThinkEveryXFrame = 25;
    private int counter = 0;
    public double VisionRange = 0.4;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
    void Update()
    {
        counter++;
        if (counter % ThinkEveryXFrame != 0)
            return;

        GameObject target= null;
        target = GameObject.FindGameObjectsWithTag("Player")[0];

        if (!((target.transform.localPosition.x > this.transform.localPosition.x - VisionRange && target.transform.localPosition.x < this.transform.localPosition.x + VisionRange)
            //&& (target.transform.localPosition.y > this.transform.localPosition.y - VisionRange && target.transform.localPosition.y < this.transform.localPosition.y + VisionRange)
            || (target.transform.localPosition.z > this.transform.localPosition.z - VisionRange && target.transform.localPosition.z < this.transform.localPosition.z + VisionRange)))
            target = null;


        if (target == null)
        {
            Debug.Log("no target found -> wander");
            Wander();
        }
        else
        {
            Debug.Log("target found -> chase");
            Chase(target, counter);
        }

    }

    void Wander()
	{
        Debug.Log("Wander");
        System.Random rng = new System.Random();

        int direction = rng.Next(0, 4);
			
		switch(direction)
		{
			case 0:
			case 1:
			case 2:
			case 3:
			default:
                //rigidbody.AddForce(force, 0f, 0f, ForceMode.Force);
			break;
		}
	}

    void Chase(GameObject target, int counter)
    {
        Debug.Log("Chasing");

        bool XIsFurthestAway = false;
        int direction = 1;

        if (System.Math.Abs(target.transform.localPosition.x - this.transform.localPosition.x) 
            > System.Math.Abs(target.transform.localPosition.z - this.transform.localPosition.z))
            XIsFurthestAway = true;

        if (XIsFurthestAway)
        {
            if (this.transform.localPosition.x  > target.transform.localPosition.x)
                direction = -1;
        }
        else
        {
            if (this.transform.localPosition.z  > target.transform.localPosition.z)
                direction = -1;
        }


        if (XIsFurthestAway)
            rigidbody.AddForce(direction*Force, 0f, 0f, ForceMode.Force);
        else
            rigidbody.AddForce(0f, 0f, direction*Force, ForceMode.Force);
    }
}
