using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject ballPrefab;
    GameObject ball;

    [SerializeField]
    GameObject UIHandler;

    [SerializeField]
    int numberOfBalls;
    bool isThereABall;
    int ballsCreated;
    public int avaliableSpawnBall;
    static List<GameObject> ballList;

    // Start is called before the first frame update
    void Start()
    {
        isThereABall = false;
        ballsCreated = 0;
        ballList = new List<GameObject>();
        avaliableSpawnBall = numberOfBalls - ballsCreated;
    }

    // Update is called once per frame
    void Update()
    {
        if (avaliableSpawnBall == 0 && ballList.Count == 0)
        {
            Lose();
        }
        if (ballList.Count == 0 && ballsCreated < numberOfBalls)
        {
            SpawnBall();
        }
    }
    void SpawnBall()
    {
        ball = Instantiate(ballPrefab, transform.position, transform.rotation);
        ballsCreated++;
        ball.GetComponent<Ball>().id = ballsCreated;
        Debug.Log("Topun idsi: " + ball.GetComponent<Ball>().id);
        ball.GetComponent<Ball>().status = Ball.Status.Alive;
        ball.name = "Ball";
        ballList.Add(ball);
        avaliableSpawnBall = numberOfBalls - ballsCreated;
    }
    
    void ClearBallList()
    {
        ballList.Clear();
        ballsCreated = 0;
    }
    public static GameObject GetLastBall()
    {
        if (ballList.Count > 0)
        {
            return ballList[ballList.Count - 1];
        }
        return null;
    }
    public static void DestroyBall()
    {
        if (ballList[ballList.Count-1].GetComponent<Ball>().status == Ball.Status.Destroyed)
        {
            Destroy(ballList[ballList.Count - 1]);
            ballList.RemoveAt(0);
        }
        
    }
    public void Restart()
    {
        isThereABall = false;
        ClearBallList();
        avaliableSpawnBall = numberOfBalls - ballsCreated;
    }
    public void Lose()
    {
        UIHandler.GetComponent<UIHandler>().Lose();
    }
}
