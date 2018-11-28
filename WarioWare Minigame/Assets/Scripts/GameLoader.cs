using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour {

    private static int index;
    public static int score;
    public static int timeLeft;
    public static int waitLeft;
    public static bool gameOn;

    public Text scoreText;
    public Text timerText;
    

	// Use this for initialization
	void Start ()
    {
        //DontDestroyOnLoad(gameObject);
        index = 0;
        score = 0;
        timeLeft = 10;
        gameOn = true;
        
        scoreText.text = "";
        timerText.text = "";

        StartCoroutine(CountDown());
        LoadNewGame();
        updateText();
    }
	
	// Update is called once per frame
	void Update () {
       
        updateText();

        
        if (gameOn == false)
        {
            gameOn = true;
            LoadNewGame();

        }
    }

    private static IEnumerator CountDown()
    {
        while (gameOn == true)
        {
            timeLeft = timeLeft - 1; //Time.deltaTime;
            Debug.Log(timeLeft);
            
            if (timeLeft <= 0 )
            {
                gameOn = false;
                LoadNewGame();
            }
               

            yield return new WaitForSeconds(1);
        }

     
      
    }

    public static void LoadNewGame()
    {
        // runs almost everytime
        if (index != 0)
        {
            SceneManager.UnloadSceneAsync(index);
            Debug.Log("Scene " + index + " Unloaded!");
            index = Random.Range(2, 5);
           
            timeLeft = 10;
            waitLeft = 3;
            gameOn = true;
           
            SceneManager.LoadScene(index, LoadSceneMode.Additive);
            Debug.Log("Scene " + index + " Loaded!");
        }
        //runs the first time
        else
        {
            index = Random.Range(2, 4);
            SceneManager.LoadScene(index, LoadSceneMode.Additive);
            Debug.Log("Scene " + index + " Loaded!");
        }
        
    }

    public static void AddScore(int newScoreValue)
    {
        score = score + newScoreValue;
        Debug.Log(score);
    }

    public void updateText()
    {
        scoreText.text = "Score: " + score;
        timerText.text = "Time Left: " + timeLeft;
    }
   
}
