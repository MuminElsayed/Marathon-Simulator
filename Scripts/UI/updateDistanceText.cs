using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class updateDistanceText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private int maxDistance;
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        maxDistance = GameManager.instance.trackDistance;
    }

    void UpdateDistanceText(float distance)
    {
        if (distance >= maxDistance) //Changed meters to miles
        {
            text.text = maxDistance + "/" + maxDistance + " MILES";
            GameManager.instance.endGame();
        } else {
            text.text = Math.Round(distance, 2).ToString() + "/" + maxDistance + " MILES";
        }
    }

    void OnEnable()
    {
        PlayerController.updateDistanceElapsedAction += UpdateDistanceText;
    }

    void OnDisable()
    {
        PlayerController.updateDistanceElapsedAction -= UpdateDistanceText;
    }
}
