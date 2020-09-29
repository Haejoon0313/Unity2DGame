using UnityEngine;

public class Background : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = PlayerMove.Instance.gameObject.transform.position;
    }
}
