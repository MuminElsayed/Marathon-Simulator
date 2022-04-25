using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableObj : MonoBehaviour
{
    [SerializeField]
    private bool disableMeshOnly;
    [SerializeField]
    private float disableTime;

    void Disable()
    {
        if (disableMeshOnly)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        } else {
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        Invoke("Disable", disableTime);
    }
}
