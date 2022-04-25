using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu, mapsMenu, skinsMenu, optionsMenu;
    public static MainMenu instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        goToMainMenu();
    }

    public void selectMaps()
    {
        mainMenu.SetActive(false);
        mapsMenu.SetActive(true);
    }

    public void selectSkins()
    {
        mainMenu.SetActive(false);
        skinsMenu.SetActive(true);
    }

    public void selectOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void goToMainMenu()
    {
        mainMenu.SetActive(true);
        mapsMenu.SetActive(false);
        skinsMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }
}
