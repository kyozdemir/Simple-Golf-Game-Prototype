using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    Text scoreText,ballText,winScoreText,loseScoreText;

    [SerializeField]
    GameObject Hole, BallSpawner,InputManager;

    [SerializeField]
    GameObject startScreen, winScreen, loseScreen, layout;
    // Start is called before the first frame update
    void Start()
    {
        BallSpawner.SetActive(false);
        InputManager.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SetScore();
        SetBallNumber();
    }
    void SetScore()
    {
        int score = Hole.GetComponent<Hole>().totalScore;
        scoreText.text = score.ToString();
    }
    void SetBallNumber()
    {
        int ballNum = BallSpawner.GetComponent<BallSpawner>().avaliableSpawnBall+1;
        ballText.text = ballNum.ToString();
    }
    public void StartGame()
    {
        BallSpawner.SetActive(true);
        InputManager.SetActive(true);
        startScreen.SetActive(false);
        layout.SetActive(true);
    }
    public void Win()
    {
        BallSpawner.SetActive(false);
        InputManager.SetActive(false);
        winScoreText.text = scoreText.text;
        layout.SetActive(false);
        winScreen.SetActive(true);
    }
    public void Lose()
    {
        BallSpawner.SetActive(false);
        InputManager.SetActive(false);
        loseScoreText.text = scoreText.text;
        layout.SetActive(false);
        loseScreen.SetActive(true);
    }
    public void Restart()
    {
        startScreen.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        BallSpawner.SetActive(false);
        InputManager.SetActive(false);
        Hole.GetComponent<Hole>().Restart();
        BallSpawner.GetComponent<BallSpawner>().Restart();
        layout.SetActive(true);
        BallSpawner.SetActive(true);
        InputManager.SetActive(true);
        
    }
}
