using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame()
    {
        //Load Game Manager Scene by name
        SceneManager.LoadScene("GameManagerHolder");
        
        //load random scene
        /*int index = Random.Range(1, 3);
        SceneManager.LoadScene(index);
        Debug.Log("Scene " + index + " Loaded!");
        */

        //load scenes in a row
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
