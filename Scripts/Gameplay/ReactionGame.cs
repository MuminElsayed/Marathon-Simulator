using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ReactionGame : MonoBehaviour
{
    [Header("0 = blue, 1 = green, 2 = yellow, 3 = red")]
    public float gameSpeed = 3f;
    [SerializeField]
    private Image outputImage;
    [SerializeField]
    private Button[] buttons;
    [SerializeField]
    private Color[] colors = new Color[4];
    private int lastColor, currentColor;
    public static Action speedUpPlayer, speedDownPlayer;

    void Start()
    {
        outputImage.gameObject.SetActive(false);
        Invoke("activateImage", 5f);
        Invoke("getNewColor", 5f);
        for (int i = 0; i < colors.Length; i++)
        {
            buttons[i].image.color = colors[i];
        }
        lastColor = -1;
    }

    void activateImage()
    {
        outputImage.gameObject.SetActive(true);
    }

    void getNewColor()
    {
        currentColor = UnityEngine.Random.Range(0, colors.Length);
        while (currentColor == lastColor)
        {
            currentColor = UnityEngine.Random.Range(0, colors.Length);
        }

        outputImage.color = colors[currentColor];
        lastColor = currentColor;

        //play new color sound
    }

    public void submitColor(int colorNum)
    {
        if (colorNum == currentColor)
        {
            //player speed up depending on reaction time
            speedUpPlayer();
            //play correct/speedup audio
        } else {
            //player slow down
            speedDownPlayer();
            //play fail/slow down audio
        }
        getNewColor();
    }
}