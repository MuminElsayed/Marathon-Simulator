using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MainCam : MonoBehaviour
{
    private GameObject player;
    [SerializeField]
    private float height, distance, tilt;
    private Vector3 defaultRotation;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        transform.position = player.transform.position - (player.transform.forward * distance) + Vector3.up * height;
        transform.forward = player.transform.forward - Vector3.up * tilt;
    }
}