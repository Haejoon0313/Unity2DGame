using UnityEngine;

public class GameManager : MonoBehaviour
{
    // score
    public int totalPoint = 0;
    public int stagePoint = 0;

    // stage
    public int stageIndex = 0;
    public GameObject[] stages;

    public int curHP;
    public int maxHP = 3;

    void Start()
    {
        totalPoint = 0;
        stagePoint = 0;
        maxHP = 3;

        curHP = maxHP;
    }

    public void NextStage()
    {
        stageIndex++;

        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void PlayerDie()
    {

    }

    void PlayerRevive()
    {

    }
}
