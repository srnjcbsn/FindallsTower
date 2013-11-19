using UnityEngine;
using System.Collections;

public class ItemMat : MonoBehaviour 
{
    public Material[] mats;
    public int interval = 1000;

    private int iter = 0;

    MeshRenderer planeRenderer;

    double time;
    double timeOld;

	// Use this for initialization
	void Start () 
    {
        planeRenderer = GetComponentInChildren<MeshRenderer>();
        planeRenderer.material = mats[iter];
        timeOld = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Change material at a given interval
        time = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
        if (time >= timeOld + interval)
        {
            timeOld = time;
            planeRenderer.material = mats[iter++];
            if (iter == mats.Length)
                iter = 0;
        }
        
	}

}
