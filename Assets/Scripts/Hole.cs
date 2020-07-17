using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField]
    GameObject UIHandler;

    int point,streak;
    public int totalScore;
    List<int> ballInfo;
    // Start is called before the first frame update
    void Start()
    {
        point = 1;
        streak = 1;
        totalScore = 0;
        ballInfo = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ballInfo.Count == 3)
        {
            Win();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            //Debug.Log("Bekle beni Veeeeeeeyyy");
            other.GetComponent<Ball>().status = Ball.Status.Destroyed;
            int ballID = other.GetComponent<Ball>().id;
            ballInfo.Add(ballID);
            Debug.Log("Listedeki Top ID'si sayısı: " + ballInfo.Count);
            StreakControl();
            BallSpawner.DestroyBall();
            Debug.Log("Toplam Skor:  "+ totalScore);
        }
    }
    void StreakControl()
    {
        if (ballInfo.Count >= 2)
        {
            Debug.Log("Buradayım! Saniyede: " + (ballInfo[ballInfo.Count - 1] - ballInfo[ballInfo.Count - 2]));
            if (ballInfo[ballInfo.Count-1] - ballInfo[ballInfo.Count -2] == 1)
            {
                Streak();
            }
            else
            {
                EndStreak();
            }
        }
        else if (ballInfo.Count == 1)
        {
            Streak();
        }
    }
    void Streak()
    {
        Debug.Log("streak oldu: " +streak );
        totalScore = totalScore + (point * streak);
        streak++;
    }
    void EndStreak()
    {
        totalScore += point;
        streak = 1;
    }
    public void Restart()
    {
        point = 1;
        streak = 1;
        totalScore = 0;
        ballInfo.Clear();
    }
    void Win()
    {
        UIHandler.GetComponent<UIHandler>().Win();
    }
}
