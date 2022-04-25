using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script adds an effect when the player's foot touches the ground
public class StepEffects : MonoBehaviour
{
    [SerializeField]
    private GameObject stepEffect, leftFoot, rightFoot;
    private int counter = 0;
    private GameObject[] steps = new GameObject[4];

    void Start()
    {
        //Create a step effect holder
        GameObject effectsHolder = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        effectsHolder.name = "StepEffects";
        //Make 4 copies of the step effect
        for (int i = 0; i < 4; i++)
        {
            GameObject obj = Instantiate(stepEffect, effectsHolder.transform);
            obj.SetActive(false);
            steps[i] = obj;
        }
    }

    public void StepEffect(int leftLeg)
    {
        if (leftLeg > 0)
        {
            steps[counter].transform.position = leftFoot.transform.position;
        } else {
            steps[counter].transform.position = rightFoot.transform.position;
        }

        steps[counter].SetActive(true);
        counter++;
        if (counter > 3)
        {
            counter = 0;
        }
    }
}