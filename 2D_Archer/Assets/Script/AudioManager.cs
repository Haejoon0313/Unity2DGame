using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // player audio
    public AudioClip playerAttack;
    public AudioClip playerSkill;
    public AudioClip playerHit;
    public AudioClip playerClear;
    public AudioClip playerCoin;
    public AudioClip playerHeart;

    // monster audio
    public AudioClip enemyHit;
    public AudioClip enemyDeath;

    // boss audio
    public AudioClip bossLeg;
    public AudioClip bossBreath;

    // bgm
    AudioSource bgm;
    public AudioClip[] gameMusic;

    // Instance
    private static AudioManager instance = null;

    void Awake()
    {
        // only one AudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // if another AudioManager, destroy
        else
        {
            Destroy(gameObject);
        }

        /// set resolution
        Screen.SetResolution(Screen.width, Screen.width * 19 / 9, true);

        // audio setting
        bgm = GetComponent<AudioSource>();
    }

    // other classes call instance
    public static AudioManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void ChangeBGM(int index)
    {
        bgm.clip = gameMusic[index];
        bgm.Play();
    }
}
