using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRandomFace : MonoBehaviour
{
    void OnEnable()
    {
        int randomNum = 1 - UnityEngine.Random.Range(0, 2) * 2;
        transform.rotation = Quaternion.Euler(90f, 90f * randomNum, 0f);
    }
}
