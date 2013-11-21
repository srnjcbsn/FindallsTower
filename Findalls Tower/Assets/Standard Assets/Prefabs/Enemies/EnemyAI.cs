using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public float Force = 10;
    public int ThinkEveryXFrame = 10;    
    public double VisionRange = 1.5;
    public float moveSpeed = 2.2f;
    public int rotationSpeed = 4; //speed of turning
    public bool WanderPackage = true;
    public bool FollowPackage = true;
    public GameObject WallPrefab;

    private int counter = 0;
    private Transform targetTransform; 
    private Transform myTransform;
    private Transform lastKnownLocation;
    //private GameObject plane;
    private TiltPlane planescript;
    private bool IsExecutingWander = false;
    private bool IsExecutingFollow = false;
    private MazeGraph.Point WanderTarget;
    private Vector3 targetPosition;
    private System.Random rng;
    private int WanderCounter = 0;

    void Awake()
    {
        myTransform = transform; //cache transform data for easy access/preformance
    }

	void Start () 
	{
        targetTransform = GameObject.FindWithTag("Player").transform; //target the player	

        //Gets access to functions in TiltPlane script
        var other = GameObject.Find("Plane").GetComponent("TiltPlane");
        planescript = (TiltPlane)other;


        rng = new System.Random();
	}
	
    void Update()
    {
        if (targetPosition == myTransform.position)
            IsExecutingWander = false;
        if (IsExecutingWander)
        {
            Wander();
            return;
        }


        GameObject target = GameObject.FindGameObjectsWithTag("Player")[0];

        if (!IsExecutingFollow)
        {
            if (StraightRoadExists(planescript.PlaneToMazeCoords(myTransform.position), planescript.PlaneToMazeCoords(target.transform.position)))
                target = null;
            //Checks if player is within enemy vision range
            if (System.Math.Abs(System.Math.Abs(target.transform.localPosition.x) - System.Math.Abs(this.transform.localPosition.x)) > VisionRange || 
                System.Math.Abs(System.Math.Abs(target.transform.localPosition.z) - System.Math.Abs(this.transform.localPosition.z)) > VisionRange)
                target = null;
        }
        
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
        IsExecutingFollow = false;
        //Debug.Log("Wander");  
        if (!IsExecutingWander)
        {
            float directionX, directionZ;
            bool targetPosIsNotValid = true;

            //Gets a random position that is not a wall and doesn't require going through walls
            while (targetPosIsNotValid)
            {
                directionX = (rng.Next(-35, 36) / 10);
                directionZ = (rng.Next(-35, 36) / 10);
                targetPosition = new Vector3(myTransform.position.x + directionX, myTransform.position.y, myTransform.position.z + directionZ);
                WanderTarget = planescript.PlaneToMazeCoords(targetPosition);
                if (planescript.GetTileOfPoint(WanderTarget).GetType() != typeof(MazeGraph.WallTile)
                    && StraightRoadExists(planescript.PlaneToMazeCoords(myTransform.position), WanderTarget))
                    targetPosIsNotValid = false;
            }
            targetPosition = planescript.MazeToPlaneCoords(WanderTarget);

            //Don't move if you are on top of the target
            if (CheckIfTooCloseToTarget(targetPosition, 0.2))
            {
                Debug.Log("Skip Wander, target too close");
                return;
            }    

            //Debug.Log("Wander target acquired");
            WanderCounter = 0;

            IsExecutingWander = true;
        }

        //Face the target
        var rot = Quaternion.LookRotation(targetPosition - myTransform.position);
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, rot, 5f * Time.deltaTime);        
        

        //move towards the player
        myTransform.position += myTransform.forward * (moveSpeed / 2) * Time.deltaTime;

        //if no success in reaching target after 100 frames, abort the wander target
        if (WanderCounter > 100)
            IsExecutingWander = false;

        WanderCounter++;
	}

    void Follow()
    {
        if (!IsExecutingFollow)
        {
            targetPosition = new Vector3(targetTransform.position.x, myTransform.position.y, targetTransform.position.z);
            lastKnownLocation = targetTransform;
            IsExecutingFollow = true;
        }
        else
            targetPosition = new Vector3(lastKnownLocation.position.x, myTransform.position.y, lastKnownLocation.position.z);

        //Debug.Log("Follow: "+targetPosition);

        if (planescript.PlaneToMazeCoords(targetPosition).X == planescript.PlaneToMazeCoords(myTransform.position).X &&
            planescript.PlaneToMazeCoords(targetPosition).Y == planescript.PlaneToMazeCoords(myTransform.position).Y)
        {
            IsExecutingFollow = false;
        }

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

    void OnCollisonEnter(Collision collison)
    {
        if (collison.gameObject.tag.Equals("Wall"))
            IsExecutingWander = false;
    }

    bool CheckIfTooCloseToTarget(Vector3 targetPosition, double precision)
    {
        if ((targetPosition.x + precision < myTransform.position.x && targetPosition.x - precision > myTransform.position.x) &&
            (targetPosition.z + precision < myTransform.position.z && targetPosition.z - precision > myTransform.position.z))
            return true;
        else
            return false;
    }

    //public static class PointClass
    //{
    //    public class Point
    //    {
    //        public Point(float x, float y)
    //        {
    //            X = x;
    //            Y = y;
    //        }
    //        public float X, Y;
    //    }
    //}
    public static MazeGraph.Point PositionToPoint(Vector3 transformPos)
    {
        return new MazeGraph.Point((int)transformPos.x, (int)transformPos.z);
    }
    public static Vector3 PointToPosition(MazeGraph.Point point)
    {
        return new Vector3(point.X, 0, point.Y);
    }

    private bool StraightRoadExists(MazeGraph.Point myPos, MazeGraph.Point targetPos)
    {
        int xDif = System.Math.Abs(myPos.X - targetPos.X), yDif = System.Math.Abs(myPos.Y - targetPos.Y);

        if (xDif > 0 && yDif > 0)
            return false;

        return planescript.GetTileOfPoint(myPos).TileIsWithinX(xDif + yDif, planescript.GetTileOfPoint(targetPos));
    }
}
