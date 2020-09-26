using UnityEngine;

public class Arrow : MonoBehaviour
{
    float lifetime = 0;

    void Update()
    {
        if (gameObject.tag == "Ultimate")
        {
            lifetime += Time.deltaTime;

            if(lifetime > 2.5f)
            {
                Destroy(gameObject);
            }
        }
    }
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
