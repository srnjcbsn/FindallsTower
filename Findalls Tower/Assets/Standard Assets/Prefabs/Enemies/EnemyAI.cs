using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public float Force = 10;
    public int ThinkEveryXFrame = 2;    
    public double VisionRange = 1.5;
    public float moveSpeed = 2.2f;
    public int rotationSpeed = 4; //speed of turning
    public bool WanderPackage = true;
    public bool FollowPackage = true;

    private int counter = 0;
    private Transform targetTransform; 
    private Transform myTransform;

    void Awake()
    {
        myTransform = transform; //cache transform data for easy access/preformance
    }

	void Start () 
	{
        targetTransform = GameObject.FindWithTag("Player").transform; //target the player	
	}
	
    void Update()
    {
        GameObject target = GameObject.FindGameObjectsWithTag("Player")[0];

        //Checks if player is within enemy vision range
        if (System.Math.Abs(System.Math.Abs(target.transform.localPosition.x) - System.Math.Abs(this.transform.localPosition.x)) > VisionRange || 
            System.Math.Abs(System.Math.Abs(target.transform.localPosition.z) - System.Math.Abs(this.transform.localPosition.z)) > VisionRange)
            target = null;

        
        if (target != null)
        {
            //If player is within range and enemy has FollowPackage enabled, execute follow
            if (FollowPackage)
            {
                Follow();
                return;
            }
        }
        
        //Checks if the enemy is allowed to think
        counter++;
        if (counter % ThinkEveryXFrame != 0)
            return;

        if (WanderPackage)
        {
            Wander();
            return;            
        }
        //TODO: other behavior
    }

    void Wander()
	{
        //Debug.Log("Wander");
        System.Random rng = new System.Random();
        Vector3 targetPosition;

        //Pick random direction to walk towards
        int directionX = rng.Next(-2, 3), directionZ = rng.Next(-2, 3);

        targetPosition = new Vector3(myTransform.position.x + directionX, myTransform.position.y, myTransform.position.z + directionZ);

        //Don't move if you are on top of the target
        if (CheckIfTooCloseToTarget(targetPosition, 0.2))
        {
            Debug.Log("Skip Wander, target too close");
            return;
        }

        //Face the player
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
        Quaternion.LookRotation(targetPosition - myTransform.position), rotationSpeed * Time.deltaTime);

        //move towards the player
        myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
	}

    void Follow()
    {
        //proof-of-concept for LoS
        var floor = GameObject.Find("FloorTest");
        floor.layer = LayerMask.NameToLayer("FloorVis");        
        
        Vector3 targetPosition = new Vector3(targetTransform.position.x, myTransform.position.y, targetTransform.position.z);

        //Debug.Log("Follow: "+targetPosition);

        //Don't move if you are on top of the target
        if (CheckIfTooCloseToTarget(targetPosition, 0.2))
        {
            Debug.Log("Can't Follow, target too close");
            return;
        }

        //rotate to look at the player
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
        Quaternion.LookRotation(targetPosition - myTransform.position), rotationSpeed * Time.deltaTime);

        //move towards the player
        myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
    }

    bool CheckIfTooCloseToTarget(Vector3 targetPosition, double precision)
    {
        if ((targetPosition.x + precision < myTransform.position.x && targetPosition.x - precision > myTransform.position.x) &&
            (targetPosition.z + precision < myTransform.position.z && targetPosition.z - precision > myTransform.position.z))
            return true;
        else
            return false;
    }    
}
