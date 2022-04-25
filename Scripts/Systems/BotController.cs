using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BotController : MonoBehaviour
{
    private CharacterController botController;
    public float moveSpeed;

    void Awake()
    {
        botController = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Char Controller
        Vector3 playerVelocity = transform.forward * moveSpeed;
        bool playerGrounded = botController.isGrounded;
        if (playerGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //Gravity
        playerVelocity.y += -9.81f * Time.deltaTime;

        //Movement
        botController.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator lerpRotate(float angle, float lerpDuration)
    {
        //Rotates player smoothly over set time
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, angle, 0), timeElapsed/lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z);
    }

    IEnumerator centerPosition(Vector3 newPos, float lerpDuration)
    {
        float timeElapsed = 0;
        //Player moving on Z-axis -> Transform.forward = (0, 0, 1/-1)
        //Player moving on X-axis -> Transform.forward = (1/-1, 0, 0)
        Vector3 posOffset = Vector3.zero;
        float variance = UnityEngine.Random.Range(-3, 4) * 0.5f; //Set and random variance for bots (from -1.5 to 1.5)

        if (transform.forward.z != 0) //Moving on Z axis (X-axis is right and left)
        {
            //Get offset of player on X axis
            posOffset = new Vector3(newPos.x - transform.position.x + variance, 0, 0);
        } else if (transform.forward.x != 0) //Moving on X axis (Z-axis is right and left)
        {
            //Get offset of player on Z axis
            posOffset = new Vector3(0, 0, newPos.z - transform.position.z + variance);
        }

        //Center player to offset over defined time period
        while (timeElapsed < lerpDuration)
        {
            transform.Translate(posOffset * (Time.deltaTime/lerpDuration));
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        //Snaps bot to exact position after loop ends (variance is 0.001 but it looks nicer in inspector ok)
        if (transform.forward.z != 0) //Moving on Z axis (X-axis is right and left)
        {
            //Snap on X-axis
            transform.position = new Vector3(newPos.x + variance, transform.position.y, transform.position.z);
        } else if (transform.forward.x != 0) //Moving on X axis (Z-axis is right and left)
        {
            //Snap on Z-axis
            transform.position = new Vector3(transform.position.x, transform.position.y, newPos.z + variance);
        }
    }

    void rightAngleTurn(float currentAngle)
    {
        int rightAngle = Mathf.RoundToInt(currentAngle/90f) * 90;
        StartCoroutine(lerpRotate(rightAngle, 2f));   
    }

    void OnTriggerEnter(Collider collider)
    {
        if (string.Equals(collider.tag, "Detector"))
        {
            gameObject.SetActive(false);
        } else if (string.Equals(collider.tag, "turnRight"))
        {
            rightAngleTurn(transform.rotation.eulerAngles.y + 90f);
        } else if (string.Equals(collider.tag, "turnLeft"))
        {
            rightAngleTurn(transform.rotation.eulerAngles.y - 90f);
        }

        if (this.enabled)
        {
            StartCoroutine(centerPosition(collider.transform.position, 2f));
        }
    }
}
