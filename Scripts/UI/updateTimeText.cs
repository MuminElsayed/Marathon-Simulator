using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class updateTimeText : MonoBehaviour
{
private TextMeshProUGUI text;
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void UpdateTimeText(float time)
    {
        text.text = ((int)time/60).ToString("00") + ":" + ((int)time % 60).ToString("00");
    }

    void OnEnable()
    {
        PlayerController.updateTimeElapsedAction += UpdateTimeText;
    }

    void OnDisable()
    {
        PlayerController.updateTimeElapsedAction -= UpdateTimeText;
    }
}