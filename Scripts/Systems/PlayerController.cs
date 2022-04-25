using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    private CharacterController charController;
    private Animator playerAnim;
    [SerializeField]
    private float moveSpeed, maxSpeed, minSpeed, defaultSpeed, distanceElapsed, timeElapsed;
    private Vector3 playerVelocity;
    public static Action<float> updatePlayerSpeedAction, updateDistanceElapsedAction, updateTimeElapsedAction;
    private IEnumerator changeSpeedEnum;
    private int playerAnimHASH;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        charController = GetComponent<CharacterController>();
        playerAnim = GetComponentInChildren<Animator>();
        playerAnimHASH = Animator.StringToHash("playerSpeed");
    }

    void Start()
    {
        //Saves starting speed then sets to zero for starting line
        defaultSpeed = moveSpeed;
        moveSpeed = 0;
    }

    void startGame()
    {
        //Lerps to default speed when game starts
        changeSpeedEnum = lerpSpeed(defaultSpeed, 5f);
        StartCoroutine(changeSpeedEnum);

        InvokeRepeating("updateStats", 0, 0.5f);
        timeElapsed = 0;
    }

    void updateStats()
    {
        if (updateTimeElapsedAction != null) //Playing canvas is active
        {
            updatePlayerSpeedAction(charController.velocity.magnitude);
            updateDistanceElapsedAction(distanceElapsed);
            updateTimeElapsedAction(timeElapsed);
        }
    }

    //Called from trackDistance when race ends
    public void endGame()
    {
        CancelInvoke("updateStats");
        StopAllCoroutines();
        moveSpeed = 0;
        uploadStats();
    }
    void uploadStats()
    {
        int averageSpeed = Mathf.RoundToInt((distanceElapsed) / timeElapsed * 1250f); //Gets average speed (MPH)
        int totalTime = Mathf.RoundToInt(timeElapsed);
        int trackNum = GameManager.instance.trackNum;

        //Sends final stats to postGameCanvas
        PostGameCanvas.instance.getStats(GameManager.instance.playerName, GameManager.instance.trackNum, averageSpeed, totalTime, GameManager.instance.trackName);
    }

    void Update()
    {
        //Char Controller
        playerVelocity = transform.forward * moveSpeed;
        distanceElapsed += moveSpeed * 0.000062f; //Change to Miles
        timeElapsed += Time.deltaTime;
        bool playerGrounded = charController.isGrounded;

        //Gravity
        if (playerGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        } else {
            playerVelocity.y += -9.81f * Time.deltaTime * 20;
        }

        //Movement
        charController.Move(playerVelocity * Time.deltaTime);

        //Animation with speed
        playerAnim.SetFloat(playerAnimHASH, moveSpeed);
    }

    public float updateSpeed()
    {
        return moveSpeed;
    }

    public float updateTime()
    {
        return timeElapsed;
    }

    public float updateDistance()
    {
        return distanceElapsed;
    }

    void changeSpeed(float changeAmount)
    {
        if (changeSpeedEnum != null)
        {
            StopCoroutine(changeSpeedEnum);
        }

        if (moveSpeed + changeAmount > maxSpeed)
        {
            changeSpeedEnum = lerpSpeed(maxSpeed, 1f);
        } else if (moveSpeed + changeAmount < minSpeed)
        {
            changeSpeedEnum = lerpSpeed(minSpeed, 1f);
        } else {
            changeSpeedEnum = lerpSpeed(moveSpeed + changeAmount, 1f);
        }
        StartCoroutine(changeSpeedEnum);
    }

    void speedUp()
    {
        changeSpeed(2);
    }

    void speedDown()
    {
        changeSpeed(-2);
    }

    IEnumerator lerpSpeed(float endSpeed, float lerpDuration)
    {
        //Changes speed over set time, then returns to default speed.
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, endSpeed, timeElapsed/lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        moveSpeed = endSpeed;

        yield return new WaitForSeconds(1f);

        timeElapsed = 0f;
        lerpDuration = 1f;

        while (timeElapsed < lerpDuration)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, defaultSpeed, timeElapsed/lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        moveSpeed = defaultSpeed;
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
        // Player moving on Z-axis -> Transform.forward = (0, 0, 1/-1)
        //Player moving on X-axis -> Transform.forward = (1/-1, 0, 0)
        Vector3 posOffset = Vector3.zero;
        
        if (transform.forward.z > 0.5f || transform.forward.z < -0.5f) //Moving on Z axis (X-axis is right and left)
        {
            //Get offset of player on X axis
            posOffset = new Vector3(newPos.x - transform.position.x, 0, 0);
        } else //Moving on X axis (Z-axis is right and left)
        {
            //Get offset of player on Z axis
            posOffset = new Vector3(0, 0, newPos.z - transform.position.z);
        }

        //Center player to offset over defined time period
        while (timeElapsed < lerpDuration)
        {
            //Player fucking goes the opposite dir
            transform.Translate(posOffset * (Time.deltaTime/lerpDuration), Space.World);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        // //Snaps player to exact position after loop ends (variance is 0.001 but it looks nicer in inspector ok)
        // if (transform.forward.z != 0) //Moving on Z axis (X-axis is right and left)
        // {
        //     //Snap on X-axis
        //     transform.position = new Vector3(newPos.x + variance, transform.position.y, transform.position.z);
        // } else if (transform.forward.x != 0) //Moving on X axis (Z-axis is right and left)
        // {
        //     //Snap on Z-axis
        //     transform.position = new Vector3(transform.position.x, transform.position.y, newPos.z + variance);
        // }
    }

    void rightAngleTurn(float currentAngle)
    {
        int rightAngle = Mathf.RoundToInt(currentAngle/90f) * 90;
        StartCoroutine(lerpRotate(rightAngle, 3f));   
    }

    void OnTriggerEnter(Collider collider)
    {
        if (string.Equals(collider.tag, "nextSection"))
        {
            GameManager.instance.NextSection(collider.transform);
            StartCoroutine(centerPosition(collider.transform.position, 2f));
        } else if (string.Equals(collider.tag, "turnRight"))
        {
            rightAngleTurn(transform.rotation.eulerAngles.y + 90f);
        } else if (string.Equals(collider.tag, "turnLeft"))
        {
            rightAngleTurn(transform.rotation.eulerAngles.y - 90f);
        }
        // StartCoroutine(centerPosition(collider.transform.position, 2f));
    }

    void OnEnable()
    {
        ReactionGame.speedUpPlayer += speedUp;
        ReactionGame.speedDownPlayer += speedDown;
        GameManager.gameStartAction += startGame;
    }

    void OnDisable()
    {
        ReactionGame.speedUpPlayer -= speedUp;
        ReactionGame.speedDownPlayer -= speedDown;
        GameManager.gameStartAction -= startGame;
    }
}
