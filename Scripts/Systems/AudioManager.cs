using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField]
    private AudioClip[] BGM;
    private AudioSource[] audioSources;
    private int lastBGM;
    private bool playMusic = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        audioSources = GetComponents<AudioSource>();

        queueBGM();
    }

    void queueBGM()
    {
        //Play music in an endless playlist
        int randomNum = UnityEngine.Random.Range(0, BGM.Length);
        while (randomNum == lastBGM)
        {
            randomNum = UnityEngine.Random.Range(0, BGM.Length);
        }

        playBGM(BGM[randomNum]);
        lastBGM = randomNum;
    }

    void playBGM(AudioClip clip)
    {
        audioSources[0].clip = clip;
        audioSources[0].Play();

        //Plays another song after this one finishes
        float songTime = clip.length + 2f;
        Invoke("queueBGM", songTime);
    }

    public void playAudio(AudioClip clip, float pitchValue)
    {
        audioSources[1].clip = clip;
        audioSources[1].pitch = pitchValue;
        audioSources[1].Play();
    }

    public void ToggleMusic(bool value)
    {
        if (value)
        {
            audioSources[0].Stop();
        } else {
            queueBGM();
        }
    }
}
