using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float maxSpeed = 3.5f;
    Rigidbody2D rigid;
    Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x* 0.2f, rigid.velocity.y);
        }

        //  Direction Sprite
        if (Input.GetButtonDown("Horizontal"))
        {
            anim.SetFloat("Facing", Input.GetAxisRaw("Horizontal"));
        }

        // Player Moving
        if(Mathf.Abs(rigid.velocity.x) < 0.2)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
        anim.SetFloat("WalkingDirection", rigid.velocity.normalized.x);
        
    }

    void FixedUpdate()
    {
        // Moving Speed
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h * 8f *Time.deltaTime, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed) // Right Max
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        if (rigid.velocity.x < maxSpeed*(-1)) // Let Max
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);
    }
}
