using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene("Stage0"+(stageIndex).ToString());
            
        }
        else
        {
            // time stop
            Time.timeScale = 0;
            Debug.Log("Game Clear!");
        }
        

        
    }

    public void PlayerDie()
    {
        // set point zero
        stagePoint = 0;

        // revive
        Invoke("ReStage", 2);
    }

    public void ReStage()
    {
        // full HP
        curHP = maxHP;

        // stage change
        SceneManager.LoadScene("Stage0" + stageIndex.ToString());
    }
}
