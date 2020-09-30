using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Text startText;
    float time;

    void Awake()
    {
        
    }

    void Start()
    {
        // set resolution
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.SetResolution(Screen.width, Screen.width * 9 / 19, true);
        
    }

    void Update()
    {

        time += Time.deltaTime;

        if (time > 1f)
        {
            startText.gameObject.SetActive(true);
            time = 0;
        }
        else if(time > 0.5f)
        {
            startText.gameObject.SetActive(false);
        }
    }

    public void FirstStage()
    {
        // go to the first stage
        SceneManager.LoadScene("Stage01", 0);
    }
}
