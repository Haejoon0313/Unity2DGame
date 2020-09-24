using UnityEngine;

public class GameManager : MonoBehaviour
{
    // score
    public int totalPoint = 0;
    public int stagePoint = 0;

    // stage
    public int stageIndex = 0;
    public GameObject[] stages;

    // player
    public PlayerMove player;
    public int curHP;
    public int maxHP = 3;

    void Start()
    {
        // set point zero
        totalPoint = 0;
        stagePoint = 0;

        // full HP
        curHP = maxHP;
    }

    public void NextStage()
    {
        if(stageIndex < stages.Length - 1)
        {
            // stage change
            stages[stageIndex].SetActive(false);
            stageIndex++;
            stages[stageIndex].SetActive(true);

            // player restore
            PlayerRevive();
            
        }
        else
        {
            // time stop
            Time.timeScale = 0;
            Debug.Log("Game Clear!");
        }
        

        // point calculation
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void PlayerDie()
    {
        // set stage point zero
        stagePoint = 0;

        // revive
        Invoke("PlayerRevive", 2);
    }

    public void PlayerRevive()
    {
        // full HP
        curHP = maxHP;

        player.Revive();
    }
}
