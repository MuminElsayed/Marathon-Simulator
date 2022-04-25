using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEater : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        collider.transform.parent.gameObject.SetActive(false);
    }
}
