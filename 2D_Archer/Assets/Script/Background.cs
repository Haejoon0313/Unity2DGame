using UnityEngine;

public class Background : MonoBehaviour
{
    void Start()
    {
        // set resolution
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.SetResolution(Screen.width, Screen.width * 9 / 19, true);
    }
    void Update()
    {
        gameObject.transform.position = PlayerMove.Instance.gameObject.transform.position;
    }
}
