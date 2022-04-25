using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;

public struct Score
{
    public string name;
    public int time, mapNum, avgSpeed;
    // public Score(string playerName, int playerTime, int mapNumber, int playerAvgSpeed)
    // {
    //     name = playerName;
    //     time = playerTime;
    //     mapNum = mapNumber;
    //     avgSpeed = playerAvgSpeed;
    // }
}

public class leaderboardHandler : MonoBehaviour
{
    public static leaderboardHandler instance;
    [SerializeField]
    private string leaderboardName = "MarathonRunnerLB";
    private string leaderboardURL;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        leaderboardURL = "https://pictorial-furnace.000webhostapp.com/" + leaderboardName + ".php";
    }

    public List<Score> RetrieveScores()
    {
        List<Score> scores = new List<Score>();
        StartCoroutine(DoRetrieveScores(scores));
        return scores;
    }
        public void PostScores(string name, int mapNum, int avgSpeed, int time)
    {
        StartCoroutine(DoPostScores(name, mapNum, avgSpeed, time));
    }

    IEnumerator DoRetrieveScores(List<Score> scores)
    {
        WWWForm form = new WWWForm();
        form.AddField("retrieve_leaderboard", "true");

        using (UnityWebRequest www = UnityWebRequest.Post(leaderboardURL, form))
        {
            yield return www.SendWebRequest();

            // if (www.isNetworkError || www.isHttpError)
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Successfully retrieved scores!");
                string contents = www.downloadHandler.text;
                using (StringReader reader = new StringReader(contents))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Score entry = new Score();
                        entry.name = line;
                        try
                        {
                            entry.avgSpeed = Int32.Parse(reader.ReadLine());
                            entry.time = Int32.Parse(reader.ReadLine());
                            entry.mapNum = Int32.Parse(reader.ReadLine());
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Invalid score: " + e);
                            continue;
                        }
                        scores.Add(entry);
                    }
                }
            }
        }
        PostGameCanvas.instance.updateLeaderboard(scores);
    }

    IEnumerator DoPostScores(string name, int mapNum, int avgSpeed, int time)
    {
        WWWForm form = new WWWForm();
        form.AddField("post_leaderboard", "true");
        form.AddField("PlayerName", name);
        form.AddField("AvgSpeed", avgSpeed);
        form.AddField("PlayerTime", time);
        form.AddField("MapNum", mapNum);

        using (UnityWebRequest www = UnityWebRequest.Post(leaderboardURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Successfully posted score!");
            }
        }
        PostGameCanvas.instance.getScore();
    }
}