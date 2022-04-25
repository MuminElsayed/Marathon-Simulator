using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCanvasManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startingCanvas, playingCanvas, postGameCanvas;
    public static GameCanvasManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        startingCanvas.SetActive(true);
        playingCanvas.SetActive(false);
        postGameCanvas.SetActive(false);
    }

    public void goToPlayingCanvas()
    {
        startingCanvas.SetActive(false);
        playingCanvas.SetActive(true);
        postGameCanvas.SetActive(false);
    }

    public void goToPostGame()
    {
        startingCanvas.SetActive(false);
        playingCanvas.SetActive(false);
        postGameCanvas.SetActive(true);
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
