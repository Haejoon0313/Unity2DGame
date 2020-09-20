using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float maxSpeed = 4.5f;
    float jumpPower = 25;
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
        // Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        // Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x* 0.2f, rigid.velocity.y);
        }

        //  Direction Sprite
        anim.SetFloat("Facing", rigid.velocity.normalized.x);

        // Player Moving
        if(Mathf.Abs(rigid.velocity.x) < 0.2)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
        
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

        // Landing Platform
        if(rigid.velocity.y < 0)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down * 2, 2, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1.6f)
                {
                    anim.SetBool("isJumping", false);
                }
            }
        }
        

    }
}
