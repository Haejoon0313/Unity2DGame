using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // score
    public int totalPoint = 0;
    public int stagePoint = 0;

    // stage
    public int stageIndex = 1;
    public int stageIndexMax = 3;
    
    // player
    public int curHP;
    public int maxHP = 3;
    public int skillNum = 1;
    public float skillCool;
    public bool enableAction = true;

    // boss
    public float curBossHP = 100;
    public float maxBossHP = 100;

    // Instance
    private static GameManager instance = null;

    void Awake()
    {
        // only one gamemanager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // if another gamemanager, destroy
        else
        {
            Destroy(gameObject);
        }
    }

    // other classes call instance
    public static GameManager Instance
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

    void Start()
    {
        // set point zero
        stagePoint = 0;

        // full HP
        curHP = maxHP;   
    }

    void Update()
    {
        // display UI
        DisplayHP();
        DisplayStage();
        DisplayScore();
        DisplaySkill();
        DisplayButton();
        if (curBossHP < maxBossHP && curBossHP > 0)
            DisplayBossHP();
        else
        {
            // boss hp inactive
            UIManager.Instance.BossUI.gameObject.SetActive(false);
        }
    }


    public void NextStage()
    {

        if(stageIndex < stageIndexMax)
        {
            // point calculation
            totalPoint += stagePoint;

            // set point zero
            stagePoint = 0;

            // full HP
            curHP = maxHP;

            // stage change
            stageIndex++;
            SceneManager.LoadScene("Stage0"+(stageIndex.ToString()));

            UIManager.Instance.startObj.gameObject.SetActive(false);
        }
        else
        {
            // time stop
            Time.timeScale = 0;
            UIManager.Instance.clearObj.gameObject.SetActive(true);
        }
    }

    void DisplayHP()
    {
        foreach (Image im in UIManager.Instance.HPImages)
        {
            im.gameObject.SetActive(false);
        }

        for(int i = 0; i < curHP; i++)
        {
            UIManager.Instance.HPImages[i].gameObject.SetActive(true);
        }
    }

    void DisplayStage()
    {
        UIManager.Instance.stageText.text = "STAGE " + stageIndex.ToString();
    }

    void DisplayScore()
    {
        UIManager.Instance.scoreText.text = (totalPoint+stagePoint).ToString();
    }

    void DisplaySkill()
    {
        UIManager.Instance.skillActive(skillNum);
    }

    void DisplayButton()
    {
        UIManager.Instance.actionActive(enableAction);
        UIManager.Instance.skillCool(skillCool);
    }

    void DisplayBossHP()
    {
        UIManager.Instance.BossUI.gameObject.SetActive(true);
        UIManager.Instance.BossHPImage.gameObject.transform.localScale = new Vector3(curBossHP / maxBossHP, 1, 1);
    }

    

    public void PlayerDie()
    {
        // set point zero
        stagePoint = 0;

        // revive
        Invoke("RetryActive", 2);

        
    }
    void RetryActive()
    {
        // time stop & retry touch enable
        Time.timeScale = 0;
        UIManager.Instance.startObj.gameObject.SetActive(true);
    }

    public void ReStage()
    {
        // full HP
        curHP = maxHP;

        // stage change
        SceneManager.LoadScene("Stage0" + stageIndex.ToString());

        // time pass & retry touch disable
        Time.timeScale = 1;
        UIManager.Instance.startObj.gameObject.SetActive(false);
    }

    public void ReStart()
    {
        // full HP
        curHP = maxHP;

        // stage change
        SceneManager.LoadScene("Menu");

        // time pass & retry touch disable
        Time.timeScale = 1;
        UIManager.Instance.clearObj.gameObject.SetActive(false);

        // reset gamemanager
        Destroy(EventSystem.Instance.gameObject);
        Destroy(UIManager.Instance.gameObject);
        Destroy(this.gameObject);
    }
}
