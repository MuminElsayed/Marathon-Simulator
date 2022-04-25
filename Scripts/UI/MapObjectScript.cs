using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MapObjectScript : MonoBehaviour
{
    [SerializeField]
    private Image mapImageObj;
    [SerializeField]
    private TextMeshProUGUI mapNameObj;
    private string mapSceneName;
    private int mapIndex;

    public void setMapDetails(Sprite sprite, string name, string sceneName, int index)
    {
        mapImageObj.sprite = sprite;
        mapNameObj.text = name;
        mapSceneName = sceneName;
        mapIndex = index;
    }

    //Called when user clicks on this obj, sets the selected effect on this obj
    public void chooseMap()
    {
        GameManager.instance.setMapSceneName(mapSceneName);
        MapSelectionMenu.setParent(this.gameObject);
    }
}