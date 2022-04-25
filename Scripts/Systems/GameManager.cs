using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameObject player, mainCam;
    
    [Header("Game References")]
    [SerializeField]
    private GameObject[] skins;
    private string mapSceneName;

    [Header("Track References")]
    public string playerName, trackName;
    public int playerSkin;
    [SerializeField]
    private GameObject[] sections;
    public int trackDistance, trackNum;
    [SerializeField]
    private int sectionLength;
    [Header("Spawn References")]
    private List<Score> allScores;
    public List<int> allSpeeds;
    [SerializeField]
    private GameObject[] carEaters;
    Vector3 spawnPos, spawnDir;
    [SerializeField]
    private float spawnDelay = 1, botSpawnDelay = 5, carSpeed = 0.2f, botSpeed = 5, carZOffset = 0;
    [SerializeField]
    private bool spawnCars, spawnBots, spawnForward;
    [SerializeField]
    private GameObject[] cars, bots;
    private List<GameObject> activeSegments;
    [SerializeField]
    private float[] carForward, carBack, botPositions;
    [SerializeField]
    private Color[] randomColors; //random colors for bots
    public static Action gameStartAction;

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

    void Start()
    {
        try {
            playerSkin = savedPlayerData.instance.playerSkinNum;
        } catch {
            Debug.Log("playerData not available");
        }
        activeSegments = new List<GameObject>();
    }

    //Called on map selection screen to change map to load
    public void setMapSceneName(string name)
    {
        mapSceneName = name;
    }

    public void loadMap()
    {
        SceneManager.LoadScene(mapSceneName);
    }

    public void StartGame()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCam = GameObject.FindWithTag("MainCamera");

        if (spawnCars)
            InvokeRepeating("spawnCar", spawnDelay, spawnDelay);

        if (spawnBots)
            InvokeRepeating("spawnBot", botSpawnDelay, botSpawnDelay);

        foreach (GameObject item in cars)
        {
            item.SetActive(false);
        }
        gameStartAction();
    }

    public void endGame()
    {
        StopAllCoroutines();
        CancelInvoke();
        GameCanvasManager.instance.goToPostGame();
        PlayerController.instance.endGame();
    }

    public void getLeaderboard()
    {
        allScores = leaderboardHandler.instance.RetrieveScores();
        allSpeeds = new List<int>();
    }

    void spawnBot()
    {
        foreach (GameObject bot in bots)
        {
            if (bot.activeInHierarchy == false)
            {
                spawnBot(bot);
                break;
            }
        }
    }

    void spawnCar()
    {
        if (spawnForward)
        {
            spawnForward = false;
        } else {
            spawnForward = true;
        }

        foreach (GameObject car in cars)
        {
            if (car.activeInHierarchy == false)
            {
                spawnCar(car, spawnForward);
                break;
            }
        }
    }

    void spawnBot(GameObject bot)
    {
        bot.transform.position = new Vector3(botPositions[UnityEngine.Random.Range(0, botPositions.Length)], bot.transform.position.y, player.transform.position.z - 7.5f);
        bot.transform.localRotation = Quaternion.Euler(0, 0, 0);

        //Give random speed boost
        bot.GetComponent<BotController>().moveSpeed = botSpeed + UnityEngine.Random.Range(0, 4);
        //give random color to bot
        bot.GetComponentInChildren<SkinnedMeshRenderer>().material.color = randomColors[UnityEngine.Random.Range(0, randomColors.Length)];
        bot.SetActive(true);
    }

    void spawnCar(GameObject car, bool forward)
    {
        if (forward)
        {
            car.transform.position = new Vector3(carForward[UnityEngine.Random.Range(0, carForward.Length)], car.transform.position.y, carEaters[0].transform.position.z + 7.5f + carZOffset);
            car.transform.localRotation = Quaternion.Euler(0, 0, 0);
        } else {
            car.transform.position = new Vector3(carBack[UnityEngine.Random.Range(0, carBack.Length)], car.transform.position.y, carEaters[1].transform.position.z - 7.5f - carZOffset);
            car.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        car.GetComponent<CarMove>().speed = carSpeed;
        car.SetActive(true);
    }

    //Called when the previous segment gets spawned
    public void NextSection(Transform triggerTransform)
    {
        GameObject enabledSegment = null;

        //Enable next segment
        //Make it random
        int randomSegment = UnityEngine.Random.Range(0, sections.Length);
        float timeElapsed = 0;

        while (sections[randomSegment].activeInHierarchy)
        {
            randomSegment = UnityEngine.Random.Range(0, sections.Length);
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= 2f)
            {
                randomSegment = 1;
                break;
            }
        }
        enabledSegment = sections[randomSegment];

        //Puts the next segment in front of the previous one, with a fixed value of segment length.
        //Minus the trigger collider offset.
        enabledSegment.transform.position = spawnPos;
        enabledSegment.transform.forward = spawnDir;
        
        enabledSegment.SetActive(true);
        activeSegments.Add(enabledSegment);

        Transform segmentTransform = enabledSegment.GetComponent<SegmentClass>().endPos;
        spawnPos = segmentTransform.position;
        spawnDir = segmentTransform.forward;

        //Removes the oldest active segment if active segments will = 3
        if (activeSegments.Count == 3)
        {
            activeSegments[0].SetActive(false);
            activeSegments.RemoveAt(0);
        }
    }

    //Changes playerSkin
    public void changePlayerSkin(int skinNum)
    {
        savedPlayerData.instance.playerSkinNum = skinNum;
        playerSkin = skinNum;
    }

    //Sends all game skins to skinPreview script
    public GameObject[] getPlayerSkins()
    {
        return skins;
    }

    //Called when a map is loaded to assign it values to gameMgr
    void getTrackInfo(GameObject[] trackSegments, int segmentLength, GameObject[] allCars, GameObject[] allBots, GameObject[] allCarEaters)
    {
        sections = trackSegments;
        sectionLength = segmentLength;
        cars = allCars;
        bots = allBots;
        carEaters = allCarEaters;
        //Also spawns the first segments and assigns values
        activeSegments.Add(sections[UnityEngine.Random.Range(0, sections.Length)]);
        activeSegments[0].SetActive(true);
        spawnPos = activeSegments[0].GetComponent<SegmentClass>().endPos.position;
        spawnDir = activeSegments[0].GetComponent<SegmentClass>().endPos.forward;
    }

    void OnEnable()
    {
        //Signs up to the trackInfo get action
        trackInfoManager.sendTrackInfo += getTrackInfo;
    }

    void OnDisable()
    {
        trackInfoManager.sendTrackInfo -= getTrackInfo;
    }
}