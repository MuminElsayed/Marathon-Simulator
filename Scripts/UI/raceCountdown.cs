using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class raceCountdown : MonoBehaviour
{
    private TextMeshProUGUI countdownText;
    private Image panel;
    [SerializeField]
    private Color[] colors = {Color.red, Color.yellow, Color.green};
    private int[] countdownNumbers = {3, 2, 1};
    private int counter;
    [SerializeField]
    private AudioClip startingBeep;

    void Start()
    {
        panel = GetComponent<Image>();
        countdownText = GetComponentInChildren<TextMeshProUGUI>();
        AdManager.instance.ShowAd();
    }

    public void startGame()
    {
        StartCoroutine("startCountdown");
    }

    private IEnumerator startCountdown()
    {
        //Countdown effects until out of range (counter > 2), then startGame
        try
        {
            panel.color = colors[counter];
            countdownText.color = colors[counter];
            countdownText.text = countdownNumbers[counter].ToString();
            AudioManager.instance.playAudio(startingBeep, 0.7f + (float)counter * 0.05f);
            counter ++;
        } catch {
            AudioManager.instance.playAudio(startingBeep, 1f);
            GameManager.instance.StartGame();
            GameCanvasManager.instance.goToPlayingCanvas();
        }
        yield return new WaitForSecondsRealtime(1f);
        StartCoroutine("startCountdown");        
    }
}