using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetIndex : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadNew()
    {
        Debug.Log("game2 loaded");
        GameLoader.LoadNewGame();
    }

    public void AddScore()
    {
        Debug.Log("more points");
        GameLoader.AddScore(1);
    }

    public void EndGame()
    {
        GameLoader.gameOn = false;
    }
}
