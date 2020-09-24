using UnityEngine;

public class Fireball : MonoBehaviour
{
    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(rigid.velocity.y < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 45);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.collider.tag == "Player")
        {
            Destroy(gameObject);
        }
        else if(collision.collider.tag == "Platform")
        {
            Destroy(gameObject);
        }
    }
}
