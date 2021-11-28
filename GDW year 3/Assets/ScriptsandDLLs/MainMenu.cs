using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");//Starts the game
    }

    public void QuitGame()
    {
        Debug.Log("Quit game!");
        Application.Quit();//Quits the application
    }

    public void Editor()
    {
        SceneManager.LoadScene("LevelLeditor");
    }
}
