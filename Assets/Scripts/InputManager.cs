using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    //All input processes are here include Restart button.
    Vector2 touchBeganPosition, touchMovedPosition, touchEndPosition;
    [SerializeField]
    Transform parentOfProjections;
    
    GameObject ball;

    [SerializeField]
    GameObject point;
    GameObject[] points;
    List<Vector3> pointPos;
    [SerializeField]
    int numberOfPoints;
    [SerializeField]
    float rotationOffset_Y = 0, rotationOffset_X=0,clampOffset = 1, vel = 8f;
    bool isDrawed;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        points = new GameObject[numberOfPoints];
        pointPos = new List<Vector3>();
        isDrawed = false;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ball == null)
        {
            FindBall();
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchBeganPosition = touch.position;
                    break;
                case TouchPhase.Moved:
                    touchMovedPosition = touch.position;
                    Rotate();
                    direction = (ball.transform.right + ball.transform.up) * vel;
                    if (parentOfProjections.childCount > 0)
                    {
                        DeleteProjections();
                        //I wrote the flags out of the methods so that we can use them more comfortably in the future.
                        isDrawed = false;
                    }
                    
                    if (!isDrawed)
                    {
                        
                        CalculateProjections();
                        
                        DrawTrajectory();
                        isDrawed = true;

                    }
                    
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    touchEndPosition = touch.position;
                    
                    Release();
                    DeleteProjections();
                    isDrawed = false;
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    break;
            }
        }
    }
    #region Methods for ball
    void Rotate()
    {
        Vector2 distance = touchBeganPosition - touchMovedPosition;
        float positionChangeX = distance.x / Screen.width;
        float positionChangeY = distance.y / Screen.height;

        positionChangeX = Mathf.Clamp(positionChangeX, -clampOffset, clampOffset);
        positionChangeY = Mathf.Clamp(positionChangeY, -clampOffset, clampOffset);
        ball.transform.rotation = Quaternion.Euler(positionChangeY * rotationOffset_X, rotationOffset_Y + (positionChangeX*90), 0f);
    }

    
    void Release()
    {
        ball.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
    }
    
    void FindBall()
    {
        ball = BallSpawner.GetLastBall();
    }
    #endregion

    #region Methods for trajectory path
    void DrawTrajectory()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = Instantiate(point, transform.position, Quaternion.identity);

            points[i].transform.position = pointPos[i];
            if (points[i].transform.position.y < 0)
            {
                points[i].transform.position = new Vector3(points[i].transform.position.x,0f,points[i].transform.position.z);
            }
            points[i].transform.parent = parentOfProjections;
            //Debug.Log(points[i].transform.position);
        }
        
    }
    //Calculating the flying time : t = 2Voy / g
    float calculateTime()
    {
        float time = (2 * vel) / Mathf.Abs(Physics.gravity.y);
        return time;
    }
    //Canculating the range : x = Vx * t
    float calculateRange(float time)
    {
        float range = vel * time;
        return range;
    }
    void CalculateProjections()
    {
        float time = calculateTime();
        float range = calculateRange(time);
        Debug.Log(time);
        pointPos.Clear();
        for (int i = 1; i <= numberOfPoints; i++)
        {
            //We have to calculate distances between projections. These distances are must be equal.
            float deltaTime = (time / numberOfPoints) * i;
            float deltaRange = (range / numberOfPoints) * i;
           
            float deltaX = (direction.x * deltaTime) + deltaRange;
            float deltaZ = (direction.z * deltaTime);

            //Y is changing due to gravity

            float vyAtGivenTime = (direction.y) + (Physics.gravity.y * deltaTime);
            float vyAverage = (vyAtGivenTime * direction.y) / 2;
            float deltaY = (vyAverage * deltaTime)/2.4f;

            Vector3 deltaPos = new Vector3(deltaX, deltaY, deltaZ);
            Vector3 projectionPos = deltaPos + ball.transform.position;
            pointPos.Add(projectionPos);
        }

    }
    void DeleteProjections()
    {
        foreach (Transform item in parentOfProjections)
        {
            Destroy(item.gameObject);
        }
        
    }
    #endregion

}