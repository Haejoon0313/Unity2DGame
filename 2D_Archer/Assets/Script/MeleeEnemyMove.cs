using UnityEngine;

public class MeleeEnemyMove : MonoBehaviour
{
    // Enemy Variables
    int speedDirection = 1;
    int ThinkTime = 2;
    public int health = 3;

    // Enemy Eyes
    Vector2 checkVec;
    RaycastHit2D rayHit;

    // Environment
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer ren;
    AudioSource audiosrc;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ren = GetComponent<SpriteRenderer>();
        audiosrc = GetComponent<AudioSource>();

        Invoke("Think", ThinkTime);
    }

    // Update is called once per frame
    void Update()
    {

        // Enemy Moving
        if(Mathf.Abs(rigid.velocity.x) < 0.05)
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
        Move();
    }

    void Move()
    {
        // Moving Speed
        rigid.velocity = new Vector2(speedDirection * 250 * Time.deltaTime, rigid.velocity.y);

        //  Direction Sprite
        anim.SetFloat("Facing", rigid.velocity.x);

        // Cliff Check
        checkVec = new Vector2(rigid.position.x + speedDirection, rigid.position.y);
        rayHit = Physics2D.Raycast(checkVec, Vector3.down * 2, 2, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Turn();
        }

        // Wall Check
        checkVec = new Vector2(rigid.position.x + speedDirection, rigid.position.y);
        rayHit = Physics2D.Raycast(checkVec, Vector3.up, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider != null)
        {
            Turn();
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            onDamaged(collision.transform.position, 1);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ultimate")
        {
            onDamaged(collision.transform.position, 10);
        }
    }
    void onDamaged(Vector2 targetPos, int dmg)
    {
        // health calculation
        health -= dmg;
        if (health <= 0)
        {
            Die();
            return;
        }

        // view alpha
        ren.color = new Color(1f, 0.1f, 0.1f, 1);

        // damaged reaction
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(0, 25), ForceMode2D.Impulse);

        // follow player
        speedDirection = dirc*(-1);

        // sound
        audiosrc.clip = AudioManager.Instance.enemyHit;
        audiosrc.Play();

        // go to normal state
        CancelInvoke();
        Invoke("Think", ThinkTime);
        Invoke("offDamaged", 0.3f);
    }

    void offDamaged()
    {
        // view normal
        ren.color = new Color(1, 1, 1, 1);
    }

    void Turn()
    {
        speedDirection *= -1;
        CancelInvoke();
        Invoke("Think", ThinkTime);
    }

    void Think()
    {
        speedDirection = Random.Range(-1, 2);
        Invoke("Think", ThinkTime);
    }

    void Die()
    {
        GameManager.Instance.KillEnemy();
        gameObject.SetActive(false);
    }
}
