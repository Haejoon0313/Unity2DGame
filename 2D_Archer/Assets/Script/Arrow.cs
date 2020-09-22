using UnityEngine;

public class Arrow : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
        else if(collision.collider.tag == "Platform")
        {
            Destroy(this.gameObject);
        }
    }
}
