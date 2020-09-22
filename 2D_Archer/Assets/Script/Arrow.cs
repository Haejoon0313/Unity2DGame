using UnityEngine;

public class Arrow : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(gameObject.tag == "Ultimate")
        {
            return;
        }

        if(collision.collider.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        else if(collision.collider.tag == "Platform")
        {
            Destroy(gameObject);
        }
    }
}
