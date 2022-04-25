using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class skinsPreview : MonoBehaviour
{
    private List<GameObject> skinPreviews;
    private int skinNumber, playerCurrentSkin;
    [SerializeField]
    private Button confirmButton;
    private TextMeshProUGUI confirmButtonText;

    void Start()
    {
        playerCurrentSkin = savedPlayerData.instance.playerSkinNum;
        confirmButtonText = confirmButton.GetComponentInChildren<TextMeshProUGUI>();
        GameObject[] skins = GameManager.instance.getPlayerSkins();
        skinPreviews = new List<GameObject>();
        foreach (GameObject skin in skins)
        {
            GameObject obj = Instantiate(skin, Vector3.zero, skin.transform.rotation, transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.SetActive(false);
            skinPreviews.Add(obj);
        }
        skinPreviews[0].SetActive(true);
        checkSkinText();
    }

    void checkSkinText()
    {
        if (skinNumber == playerCurrentSkin)
        {
            confirmButtonText.text = "Selected";
            confirmButton.enabled = false;
        } else {
            confirmButtonText.text = "Confirm";
            confirmButton.enabled = true;
        }
    }

    public void nextSkin()
    {
        skinPreviews[skinNumber].SetActive(false);
        if (skinNumber == skinPreviews.Count - 1)
        {
            skinNumber = 0;
        } else {
            skinNumber ++;
        }
        skinPreviews[skinNumber].SetActive(true);
        checkSkinText();
    }

    public void prevSkin()
    {
        skinPreviews[skinNumber].SetActive(false);
        if (skinNumber == 0)
        {
            skinNumber = skinPreviews.Count - 1;
        } else {
            skinNumber --;
        }
        skinPreviews[skinNumber].SetActive(true);
        checkSkinText();
    }

    //Save skin selection
    public void confirmSkin()
    {
        GameManager.instance.changePlayerSkin(skinNumber);
        playerCurrentSkin = skinNumber;
        checkSkinText();
        MainMenu.instance.goToMainMenu();
    }
}
