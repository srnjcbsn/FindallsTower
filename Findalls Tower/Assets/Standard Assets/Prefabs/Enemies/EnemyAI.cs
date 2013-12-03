using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public float Force = 10;
    public int ThinkEveryXFrame = 10;    
    public double VisionRange = 1.5;
    public float moveSpeed = 1.1f;
    public int rotationSpeed = 4; //speed of turning
    public bool WanderPackage = true;
    public bool FollowPackage = true;

    private int counter = 0;
    private Transform targetTransform; 
    private Transform myTransform;
    private Vector3 lastKnownLocation;
    //private GameObject plane;
    private PlaneScript planescript;
    private TileScript tilescript;
    private bool IsExecutingWander = false;
    private bool IsExecutingFollow = false;
    private MazeGraph.Point WanderTarget;
    private Vector3 targetPosition = new Vector3(-9999, -9999, -9999);
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
        var plane = GameObject.Find("Plane").GetComponent("PlaneScript");
        planescript = (PlaneScript)plane;
        float x = 0, z = 0;

        while (planescript.PlaneToMazeCoords(transform.position).GetType() == typeof(MazeGraph.WallTile))
        {
            Debug.Log("Enemy started on wall tile, repositioning.");
            x += 0.1f;
            z += 0.1f;
            transform.Translate(x, 1, z);
        }

        rng = new System.Random();
	}
	
    void Update()
    {
        if (planescript.PlaneToMazeCoords(targetPosition) == planescript.PlaneToMazeCoords(myTransform.position))
            IsExecutingWander = false;
        if (IsExecutingWander)
        {
            Wander();
            return;
        }

        GameObject target = GameObject.FindGameObjectsWithTag("Player")[0];
        MazeGraph.Point targetPoint = planescript.PlaneToMazeCoords(target.transform.position);
        
        //Debug.Log("" + planescript.PlaneToMazeCoords(target.transform.localPosition));

        if (planescript.GetTileOfPoint(targetPoint).GetType() != typeof(MazeGraph.WallTile))
        {
            if (!IsExecutingFollow)
            {
                //Debug.Log("Can try to see player");
                //Checks if a straightline exists to the player (aka the enemy have LoS to the player) 
                if (!StraightRoadExists(planescript.PlaneToMazeCoords(myTransform.position), targetPoint))
                    target = null;
                //Checks if player is within enemy vision range
                if (target != null)
                {
                    if (System.Math.Abs(System.Math.Abs(target.transform.localPosition.x) - System.Math.Abs(myTransform.localPosition.x)) > VisionRange ||
                        System.Math.Abs(System.Math.Abs(target.transform.localPosition.z) - System.Math.Abs(myTransform.localPosition.z)) > VisionRange)
                        target = null;
                }
            }
            //Debug.Log("" + System.Math.Abs(System.Math.Abs(target.transform.localPosition.x) - System.Math.Abs(myTransform.localPosition.x)));
            //Debug.Log("" + System.Math.Abs(System.Math.Abs(target.transform.localPosition.z) - System.Math.Abs(myTransform.localPosition.z)));
            if (target != null)
            {
                //If player is within range and enemy has FollowPackage enabled, execute follow
                if (FollowPackage)
                {
                    Follow();
                    return;
                }
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
            int negX, negZ;
            bool targetPosIsNotValid = true;

            //Gets a random position that is not a wall and doesn't require going through walls
            while (targetPosIsNotValid)
            {                
                directionX = (float)(rng.NextDouble());
                directionZ = (float)(rng.NextDouble());
                negX = rng.Next(-1, 2);
                negZ = rng.Next(-1, 2);
                targetPosition = new Vector3(myTransform.position.x + (negX * directionX), myTransform.position.y, myTransform.position.z + (negZ * directionZ));
                WanderTarget = planescript.PlaneToMazeCoords(targetPosition);
                //Debug.Log("Looping..." + WanderTarget + " " + planescript.PlaneToMazeCoords(myTransform.position));
                if (planescript.GetTileOfPoint(WanderTarget).GetType() != typeof(MazeGraph.WallTile))
                {
                    //Debug.Log("target not wall");
                    if (StraightRoadExists(planescript.PlaneToMazeCoords(myTransform.position), WanderTarget))
                        targetPosIsNotValid = false;
                }
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
        Quaternion rot = Quaternion.LookRotation(targetPosition - myTransform.position);
        if (Quaternion.Angle(myTransform.rotation, rot) > 0.1)
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, rot, 10f * Time.deltaTime);
        

        //move towards the target
        myTransform.position += myTransform.forward * (moveSpeed / 2) * Time.deltaTime;

        //if no success in reaching target after 120 frames, abort the wander target
        if (WanderCounter > 120)
            IsExecutingWander = false;

        WanderCounter++;
	}

    void Follow()
    {
        //aqcuire a target if not already following a target
        if (!IsExecutingFollow)
        {
            targetPosition = new Vector3(targetTransform.position.x, myTransform.position.y, targetTransform.position.z);
            lastKnownLocation = new Vector3(targetTransform.position.x, myTransform.position.y, targetTransform.position.z);
            IsExecutingFollow = true;
        }
        else
            targetPosition = new Vector3(lastKnownLocation.x, myTransform.position.y, lastKnownLocation.z);

        //Debug.Log("Follow: " + targetPosition + " is executing a follow order: " + IsExecutingFollow);

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
        //Debug.Log("only xDif or yDif is larger than 0. Sum is: " + (xDif + yDif) + " target/my-pos x/y: " + targetPos.X + " " + targetPos.Y + " " + myPos.X + " " + myPos.Y);
        return planescript.AreTilesWithinRange(planescript.GetTileOfPoint(myPos), planescript.GetTileOfPoint(targetPos), xDif + yDif);
    }
}
