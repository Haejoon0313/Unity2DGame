using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    // Player Variables
    float maxSpeed = 12;
    float jumpPower = 50;
    float speedDirection = 1;
    bool controlDisabled = false;

    // Arrow Variables
    public GameObject arrowObj;
    public GameObject firearrowObj;
    float curShotDelay;
    float maxShotDelay = 0.7f;

    // Environment
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
        Jump();
        Condition();
        Reload();
        Attack();
        UltimateAttack();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // Moving Speed
        float h = Input.GetAxisRaw("Horizontal");

        // If damaged, player control impossible
        if (!controlDisabled)
        {
            rigid.AddForce(new Vector2(h * 350* Time.deltaTime, 0), ForceMode2D.Impulse);
        }

        if (rigid.velocity.x > maxSpeed) // Right Max
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        if (rigid.velocity.x < maxSpeed * (-1)) // Left Max
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //  Direction Sprite
        if (h > 0)
        {
            speedDirection = 1;
        }
        else if(h < 0)
        {
            speedDirection = -1;
        }
        anim.SetFloat("Facing", speedDirection);

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

    void Condition()
    {
        // Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.2f, rigid.velocity.y);
        }

        // Player Moving Check
        if (Mathf.Abs(rigid.velocity.x) < 0.2)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
    }

    void Jump()
    {
        // Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping") && !controlDisabled)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;

    }

    void Attack()
    {
        // if reload enough
        if (curShotDelay < maxShotDelay)
        {
            return;
        }

        // Attack
        if (Input.GetButton("Fire1") && !controlDisabled)
        {
            // animation
            anim.SetTrigger("Shoot");
            controlDisabled = true;

            // make delay zero
            curShotDelay = 0;

            Invoke("Shoot", 0.5f);
        }
    }

    void Shoot()
    {
        // arrow initialize
        GameObject arrow = Instantiate(arrowObj, transform.position, transform.rotation);
        Rigidbody2D arrowrigid = arrow.GetComponent<Rigidbody2D>();
        SpriteRenderer arrowren = arrow.GetComponent<SpriteRenderer>();

        // shoot to player facing
        if (speedDirection < 0)
        {
            arrowren.flipX = true;
        }
        arrowrigid.AddForce(Vector2.right * 20 * speedDirection, ForceMode2D.Impulse);

        Invoke("availableControl", 0.2f);
    }

    void UltimateAttack()
    {
        // Attack
        if (Input.GetButtonDown("Fire2") && !controlDisabled)
        {
            // animation
            anim.SetTrigger("Shoot");
            controlDisabled = true;


            Invoke("Cast", 0.5f);
        }
    }
    void Cast()
    {
        // arrow initialize
        GameObject arrow = Instantiate(firearrowObj, transform.position, transform.rotation);
        Rigidbody2D arrowrigid = arrow.GetComponent<Rigidbody2D>();
        SpriteRenderer arrowren = arrow.GetComponent<SpriteRenderer>();

        // shoot to player facing
        if (speedDirection < 0)
        {
            arrowren.flipX = true;
        }
        arrowrigid.AddForce(Vector2.right * 10 * speedDirection, ForceMode2D.Impulse);

        Invoke("availableControl", 0.2f);
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
