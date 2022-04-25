using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public class mapObject
{
    public string MapName;
    public UnityEngine.Object mapScene;
    public int mapIndex;
    public Sprite sprite;
    // public sprite
}

public class MapSelectionMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mapsHolder, mapPreviewPrefab, selectedEffectObj;
    [SerializeField]
    private mapObject[] allMaps;
    private List<GameObject> allMapsList;
    public static Action<GameObject> setParent;

    void Start()
    {
        allMapsList = new List<GameObject>();
        //creates a map object for each map in the list, then adds the objects in a list to reference later
        foreach (var map in allMaps)
        {
            GameObject obj = Instantiate(mapPreviewPrefab, mapsHolder.transform);
            obj.GetComponent<MapObjectScript>().setMapDetails(map.sprite, map.MapName, map.mapScene.name, map.mapIndex);
            allMapsList.Add(obj);
        }
    }

    //Called when user clicks on a map preview, sets selected effect to the selected map
    public void selectMapEffect(GameObject obj)
    {
        selectedEffectObj.transform.SetParent(obj.transform, false);
        selectedEffectObj.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        selectedEffectObj.SetActive(true);
    }

    void OnEnable()
    {
        setParent += selectMapEffect;
    }

    void OnDisable()
    {
        setParent -= selectMapEffect;
    }
}