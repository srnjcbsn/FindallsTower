using UnityEngine;
using System.Collections;

public class ExitTile : MonoBehaviour 
{
    private static bool exit = false;
    public static bool Exit { get { return exit; } }

    void OnCollisionEnter(Collision collision)
    {

        Transform collidingTransform = collision.transform;

        if (collidingTransform.tag == "Player")
        {
            exit = true;
        }

        
    }
}
