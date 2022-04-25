using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class savedPlayerData : MonoBehaviour
{
    public static savedPlayerData instance;

    public string playerName = "Player123";
    public int playerSkinNum = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void OnEnable()
    {
        //Load player data
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("savedPlayerData"), this);
    }

    void OnDisable()
    {
        //Save player data
        string jsonData = JsonUtility.ToJson(this, false);
        PlayerPrefs.SetString("savedPlayerData", jsonData);
        PlayerPrefs.Save();
    }
}
