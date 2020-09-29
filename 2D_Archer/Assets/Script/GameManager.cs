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
    AudioSource audiosrc;

    // boss
    public float curBossHP = 100;
    public float maxBossHP = 100;

    // Instance
    private static GameManager instance = null;

    void Awake()
    {
        audiosrc = GetComponent<AudioSource>();

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
    
    public void PlayerClear()
    {
        Invoke("NextStage", 2);
    }

    void NextStage()
    {

        if(stageIndex < stageIndexMax)
        {
            // point calculation
            totalPoint += stagePoint;

            // set point zero
            stagePoint = 0;

            // player init
            curHP = maxHP;
            PlayerMove.Instance.gameObject.transform.position = new Vector3(0, 4.5f, 0);

            // stage change
            stageIndex++;
            SceneManager.LoadScene("Stage0"+(stageIndex.ToString()));

            UIManager.Instance.startObj.gameObject.SetActive(false);

            // time pass
            Time.timeScale = 1;
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

        // bgm change
        AudioManager.Instance.ChangeBGM(2);
    }
    void RetryActive()
    {
        // time stop & retry touch enable
        Time.timeScale = 0;
        UIManager.Instance.startObj.gameObject.SetActive(true);
    }

    public void ReStage()
    {
        // player init
        curHP = maxHP;
        PlayerMove.Instance.gameObject.transform.position = new Vector3(0, 4.5f, 0);
        PlayerMove.Instance.rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        PlayerMove.Instance.availableControl();
        PlayerMove.Instance.gameObject.layer = 9;

        // stage load
        SceneManager.LoadScene("Stage0" + stageIndex.ToString());

        // time pass & retry touch disable
        Time.timeScale = 1;
        UIManager.Instance.startObj.gameObject.SetActive(false);

        // bgm restart
        AudioManager.Instance.ChangeBGM(0);
    }

    public void ReStart()
    {
        // time pass & retry touch disable
        Time.timeScale = 1;
        UIManager.Instance.clearObj.gameObject.SetActive(false);

        // reset gamemanager
        Destroy(EventSystem.Instance.gameObject);
        Destroy(UIManager.Instance.gameObject);
        Destroy(AudioManager.Instance.gameObject);
        Destroy(PlayerMove.Instance.gameObject);

        // stage change
        SceneManager.LoadScene("Menu");

        Destroy(this.gameObject);
    }

    public void KillEnemy()
    {
        stagePoint += 100;
        // sound
        audiosrc.clip = AudioManager.Instance.enemyDeath;
        audiosrc.Play();
    }
}
