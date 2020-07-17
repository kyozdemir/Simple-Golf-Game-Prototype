using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            other.GetComponent<Ball>().status = Ball.Status.Destroyed;
            BallSpawner.DestroyBall();
        }
    }
}
