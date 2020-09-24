using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject player;

    float camera_x_pos;
    float camera_y_pos;

    float player_x_pos;
    float player_y_pos;

    float x_diff;
    float y_diff;

    void Start()
    {
        gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }

    // Update is called once per frame
    void Update()
    {
        camera_x_pos = gameObject.transform.position.x;
        camera_y_pos = gameObject.transform.position.y;

        player_x_pos = player.transform.position.x;
        player_y_pos = player.transform.position.y;

        x_diff = camera_x_pos - player_x_pos;
        y_diff = camera_y_pos - player_y_pos;

        if(x_diff > 3)
        {
            camera_x_pos = player_x_pos + 3;
        }
        else if (x_diff < -3)
        {
            camera_x_pos = player_x_pos - 3;
        }

        if (y_diff > 3)
        {
            camera_y_pos = player_y_pos + 3;
        }
        else if (y_diff < -3)
        {
            camera_y_pos = player_y_pos - 3;
        }

        gameObject.transform.position = new Vector3(camera_x_pos, camera_y_pos, -10);
    }
}
