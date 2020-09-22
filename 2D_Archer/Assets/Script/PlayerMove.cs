using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    float maxSpeed = 12;
    float jumpPower = 50;
    float speedBoost = 350;
    bool controlDisabled = false;
    
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer ren;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ren = GetComponent<SpriteRenderer>();
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

        // Attack
        if (Input.GetButtonDown("Fire1") && !anim.GetBool("isAttacking"))
        {
            playerAttack();
            anim.SetBool("isAttacking", true);
        }

        // Player Moving
        if (Mathf.Abs(rigid.velocity.x) < 0.2)
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
        Debug.Log(h);
        if (!controlDisabled)
        {
            rigid.AddForce(new Vector2(h * speedBoost * Time.deltaTime, 0), ForceMode2D.Impulse);
        }

        if (rigid.velocity.x > maxSpeed) // Right Max
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        if (rigid.velocity.x < maxSpeed*(-1)) // Left Max
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);

        //  Direction Sprite
        anim.SetFloat("Facing", h);

        // Landing Platform
        if (rigid.velocity.y < 0)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down * 2, 2, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1.6f)
                {
                    anim.SetBool("isJumping", false);
                }
            }
            else
            {
                anim.SetBool("isJumping", true);
            }
        }
    }

    void playerAttack()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            onDamaged(collision.transform.position);
        }
    }

    void onDamaged(Vector2 targetPos)
    {
        // immortal active
        gameObject.layer = 10;

        // view alpha
        ren.color = new Color(1f, 0.1f, 0.1f, 1);

        // damaged reaction
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc*50, 25), ForceMode2D.Impulse);
        
        // disable control for a while
        controlDisabled = true;

        Invoke("availableControl", 0.5f);
        Invoke("offDamaged", 1f);

    }

    void offDamaged()
    {
        // immortal inactive
        gameObject.layer = 9;
        
        // view normal
        ren.color = new Color(1, 1, 1, 1);
    }

    void availableControl()
    {
        controlDisabled = false;
    }

}
