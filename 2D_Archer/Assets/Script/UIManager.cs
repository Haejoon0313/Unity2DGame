using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // UI
    public Text scoreText;
    public Text stageText;
    public Image[] HPImages;

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
}
