using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;

    public int playerHealth;

    void Start()
    {
        totalPoint = 0;
        stagePoint = 0;
        stageIndex = 0;

        playerHealth = 3;
    }

    void Update()
    {
        
    }
}
