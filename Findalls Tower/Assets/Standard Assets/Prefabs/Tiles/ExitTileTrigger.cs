using UnityEngine;
using System.Collections;

public class ExitTileTrigger : MonoBehaviour 
{
    public static bool exit = false;

    void OnCollisionEnter(Collision collision)
    {

        Transform collidingTransform = collision.transform;

        if (collidingTransform.tag == "Player")
        {
            exit = true;
        }

        
    }
}
