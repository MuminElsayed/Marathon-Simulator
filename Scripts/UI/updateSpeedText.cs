using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class updateSpeedText : MonoBehaviour
{
    private TextMeshProUGUI text;
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void UpdateSpeedText(float speed)
    {
        text.text = "Speed: " + Math.Round(speed, 2).ToString() + " mph";
    }

    void OnEnable()
    {
        PlayerController.updatePlayerSpeedAction += UpdateSpeedText;
    }

    void OnDisable()
    {
        PlayerController.updatePlayerSpeedAction -= UpdateSpeedText;
    }
}
