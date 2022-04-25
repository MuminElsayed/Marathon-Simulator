using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField]
    private float spawnDelay = 3f;
    [SerializeField]
    private float botSpeed = 5f;
    [SerializeField]
    private GameObject[] bots;
    [SerializeField]
    private float[] botPositions;
    [SerializeField]
    private Color[] randomColors;

    void Start()
    {
        InvokeRepeating("spawnBot", spawnDelay, spawnDelay);
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

    void spawnBot(GameObject bot)
    {
        bot.transform.localPosition = new Vector3(botPositions[UnityEngine.Random.Range(0, botPositions.Length)], bot.transform.localPosition.y, 0);
        bot.transform.localRotation = Quaternion.Euler(0, 0, 0);

        bot.GetComponent<BotController>().moveSpeed = botSpeed + UnityEngine.Random.Range(0, 4);
        bot.GetComponentInChildren<SkinnedMeshRenderer>().material.color = randomColors[UnityEngine.Random.Range(0, randomColors.Length)];
        bot.SetActive(true);
    }
}