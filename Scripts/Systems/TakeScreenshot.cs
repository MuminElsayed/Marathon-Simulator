using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TakeScreenshot : MonoBehaviour
{
    [SerializeField]
    private string path;
    public bool takeScreenshot;
    

    void Update()
    {
        if (takeScreenshot == true)
        {
            screenshot();
        }
    }
    public void screenshot()
    {
        ScreenCapture.CaptureScreenshot(path + SceneManager.GetActiveScene().name + Time.time + ".png");
        takeScreenshot = false;
        print("Screenshot taken at " + path);
    }
}
