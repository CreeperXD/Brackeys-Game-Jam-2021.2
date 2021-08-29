using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void MainMenuButton() {
        SceneManager.LoadScene("Menu");
    }
    
    public void Level1Button() {
        SceneManager.LoadScene("Level1");
    }

    public void Level2Button() {
        SceneManager.LoadScene("Level2");
    }
    
    public void Level3Button() {
        SceneManager.LoadScene("Level3");
    }
    
    public void Level4Button() {
        SceneManager.LoadScene("Level4");
    }

    public void QuitButton() {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }
}
