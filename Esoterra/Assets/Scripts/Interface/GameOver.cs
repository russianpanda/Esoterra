using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public bool gameOver = false;

    public GameObject gameOverUI;

    private AudioSource gameOverMusic;

    void Start(){
        Cursor.visible = true;
        gameOverMusic = gameObject.GetComponent<AudioSource>();
        gameOverMusic.Play();
    }
    
    void Update()
    {
        stopGame();
    }

    void stopGame(){
        gameOverUI.SetActive(true);
        //show the ending screen 
        
        Time.timeScale = 0f;
        //freeze game
        gameOver = true;

    }

    public void LoadMenu(){
        Time.timeScale = 1f; //unpauses game
        Debug.Log("Main menu");
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void quitGame(){
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
