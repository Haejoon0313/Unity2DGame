﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // UI
    public Text scoreText;
    public Text stageText;
    public Button attackButton;
    public Button jumpButton;
    public Button skillButton;
    public Image skillCoolImage;
    public Image[] HPImages;
    public GameObject BossUI;
    public Image BossHPImage;
    public GameObject startObj;
    public GameObject clearObj;

    // Instance
    private static UIManager instance = null;

    void Awake()
    {
        // only one UIManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // if another UIManager, destroy
        else
        {
            Destroy(gameObject);
        }

    }

    // other classes call instance
    public static UIManager Instance
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

    /*public void FirstStage()
    {
        // go to the first stage
        SceneManager.LoadScene("Stage01");
    }*/

    

        public void skillActive(int skillNum)
    {
        // unable to touch skill when zero
        if(skillNum == 0)
        {
            skillButton.interactable = false;
        }
        else
        {
            skillButton.interactable = true;
        }
    }

    public void skillCool(float percent)
    {
        skillCoolImage.fillAmount = 1.0f - percent;
    }

    public void actionActive(bool active)
    {
        attackButton.interactable = active;
        jumpButton.interactable = active;
        skillButton.interactable = active;
    }



}
