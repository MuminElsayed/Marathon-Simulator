using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

public class PostGameCanvas : MonoBehaviour
{
    public static PostGameCanvas instance;
    [SerializeField]
    private TextMeshProUGUI statsPanelText, playerRanksTxt, playerNamesTxt, playerSpeedsTxt, playerTimesTxt;
    private string playerName, mapName;
    [SerializeField]
    private GameObject statsPanel, leaderboardPanel, optionsMenu;
    [SerializeField]
    private int leaderboardMaxPages, currentLBpage = 1, playerTotalTime, mapNum, playerAvgSpeed;
    public List<Score> scores;
    [SerializeField]
    private Animator LBanim;
    private bool postedScore;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        postedScore = false;
        openStatsPanel();
    }

    void Start()
    {
        leaderboardMaxPages = 5;
        currentLBpage = 1;
        changeLBPages(currentLBpage);
        scores = new List<Score>();
        playerName = PlayerPrefs.GetString("playerName", "Usian Bolt");
    }

    public void getStats(string name, int mapNumber, int speed, int time, string trackName)
    {
        playerName = name;
        mapNum = mapNumber;
        playerAvgSpeed = speed;
        playerTotalTime = time;
        mapName = trackName;
        setStats(); //Shows stats to player;
    }

    void setStats()
    {
        statsPanelText.text = 
        "Map Name: " + mapName + "\n" + 
        "Distance Ran: " + GameManager.instance.trackDistance + " miles" + "\n" + 
        "Average Speed: " + playerAvgSpeed.ToString() + " mph" + "\n" + 
        "Time: " + (playerTotalTime/60).ToString("00") + ":" + (playerTotalTime % 60).ToString("00") + "\n";
    }

    public void openStatsPanel()
    {
        statsPanel.SetActive(true);
        leaderboardPanel.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void openLeaderboard()
    {
        statsPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
        postScore();
        optionsMenu.SetActive(false);
    }

    public void restartTraining()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void openOptionsMenu()
    {
        statsPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void nextLeaderboardPage()
    {
        if (currentLBpage == leaderboardMaxPages)
        {
            currentLBpage = 1;
        } else {
            currentLBpage++;
        }
        changeLBPages(currentLBpage);
    }

    public void prevLeaderboardPage()
    {
        if (currentLBpage == 1)
        {
            currentLBpage = leaderboardMaxPages;
        } else {
            currentLBpage--;
        }
        changeLBPages(currentLBpage);
    }

    void changeLBPages(int pageNumber)
    {
        playerRanksTxt.pageToDisplay = currentLBpage;
        playerNamesTxt.pageToDisplay = currentLBpage;
        playerSpeedsTxt.pageToDisplay = currentLBpage;
        playerTimesTxt.pageToDisplay = currentLBpage;
    }

    public void getScore()
    {
        scores = leaderboardHandler.instance.RetrieveScores();
    }

    public void updateLeaderboard(List<Score> updatedScores)
    {
        scores = updatedScores;
        int counter = 1;
        foreach (Score score in scores)
        {
            if (counter == 1)
            {
                //Titles
                playerRanksTxt.text = "#\n";
                playerNamesTxt.text = "Name\n";
                playerSpeedsTxt.text = "Average Speed\n";
                playerTimesTxt.text = "Time\n";
            }
            
            playerRanksTxt.text += counter + ".\n";
            playerNamesTxt.text += score.name + "\n";
            playerSpeedsTxt.text += score.avgSpeed + "\n";
            playerTimesTxt.text += (score.time/60).ToString("00") + ":" + (score.time % 60).ToString("00") + "\n";
            counter++;
        }
        leaderboardMaxPages = Mathf.CeilToInt((float)scores.Count/7f);
        LBanim.SetBool("Loading", false);
    }

    public void postScore()
    {
        if (!postedScore && playerTotalTime != 0) //Didn't post/get scores list yet
        {
            leaderboardHandler.instance.PostScores(playerName, mapNum, playerAvgSpeed, playerTotalTime);
            getScore();
            postedScore = true;
        } else if (scores.Count == 0){ //Scores list is empty
            getScore();
        } else { //Already loaded scores, no changes, just update values and reset counters
            updateLeaderboard(scores);
        }
    }

    /* 
        Tower Sample with 4 floors:
                xxx0xxx
                xx000xx
                x00000x
                0000000
    */
    public static string[] TowerBuilder(int nFloors)
    {
        string[] tower = new string[nFloors];
        for (int i = 0; i < nFloors; i++)
        {
            for (int j = 0; j < nFloors * 2 - 1; j++) //Incriments through a floor
            {
                int counter = 0; 
                if (counter != (i * 2) + 1) //Not on block spot
                {
                    //Add space
                    tower[i] += ' ';
                } else { //On block spot
                    //Add block
                    tower[i] += '*';
                }
            }
        }

        return tower;
    }
}