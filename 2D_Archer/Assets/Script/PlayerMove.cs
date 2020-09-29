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
    float shootDirection = 1;
    bool controlDisabled = true;

    // Arrow Variables
    public GameObject arrowObj;
    public GameObject firearrowObj;
    float curShotDelay = 0;
    float maxShotDelay = 0.7f;
    float curChargeTime = 0f;
    float maxChargeTime = 10f;
    int curChargeNum = 1;
    int maxChargeNum = 1;

    // mobile key mapping
    int left_value;
    int right_value;
    bool jump_value;
    bool attack_value;
    bool skill_value;

    // Environment
    public Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer ren;
    AudioSource audiosrc;

    // Instance
    private static PlayerMove instance = null;

    void Awake()
    {
        // only one PlayerMove
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // if another PlayerMove, destroy
        else
        {
            Destroy(gameObject);
        }

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ren = GetComponent<SpriteRenderer>();
        audiosrc = GetComponent<AudioSource>();

        Invoke("availableControl", 1f);
    }

    // other classes call instance
    public static PlayerMove Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
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
        float h = left_value + right_value;

        // If damaged, player control impossible
        if (!controlDisabled)
        {
            rigid.AddForce(new Vector2(h * 350 * Time.deltaTime, 0), ForceMode2D.Impulse);
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
        else if (h < 0)
        {
            speedDirection = -1;
        }
        if (!controlDisabled)
        {
            anim.SetFloat("Facing", speedDirection);
        }

        // Landing Platform
        if (rigid.velocity.y < -5)
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
        if ((left_value + right_value) == 0)
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.1f, rigid.velocity.y);
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
        if (jump_value && !anim.GetBool("isJumping") && !controlDisabled)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
    }

    void Reload()
    {
        // Normal Attack Delay
        curShotDelay += Time.deltaTime;

        // Store Ultimate Attack until 1
        if (curChargeNum < maxChargeNum)
        {
            curChargeTime += Time.deltaTime;
            GameManager.Instance.skillCool = curChargeTime / maxChargeTime;
        }
        else
        {
            curChargeTime = 0;
            GameManager.Instance.skillCool = 1;
        }
        if (curChargeTime >= maxChargeTime)
        {
            curChargeNum++;
            GameManager.Instance.skillNum = curChargeNum;
        }
        
    }

    void Attack()
    {
        // if reload enough
        if (curShotDelay < maxShotDelay)
        {
            return;
        }

        // Attack
        if (attack_value && !controlDisabled)
        {
            // animation
            anim.SetTrigger("Shoot");
            shootDirection = speedDirection;
            disableControl();

            // make delay zero
            curShotDelay = 0;

            Invoke("Shoot", 0.45f);
        }
    }

    void Shoot()
    {
        // arrow initialize
        GameObject arrow = Instantiate(arrowObj, transform.position, transform.rotation);
        Rigidbody2D arrowrigid = arrow.GetComponent<Rigidbody2D>();
        SpriteRenderer arrowren = arrow.GetComponent<SpriteRenderer>();

        // shoot to player facing
        if (shootDirection < 0)
        {
            arrowren.flipX = true;
        }
        arrowrigid.AddForce(Vector2.right * 20 * shootDirection, ForceMode2D.Impulse);

        // sound
        audiosrc.clip = AudioManager.Instance.playerAttack;
        audiosrc.Play();

        availableControl();
    }

    void UltimateAttack()
    {
        // if ultimate chance exist
        if (curChargeNum <= 0)
        {
            return;
        }

        // Attack
        if (skill_value && !controlDisabled)
        {


            // animation
            anim.SetTrigger("Shoot");
            shootDirection = speedDirection;
            disableControl();

            // use charging ultimate
            curChargeNum--;
            GameManager.Instance.skillNum = curChargeNum;

            Invoke("Cast", 0.45f);
        }
    }
    public void Cast()
    {
        // arrow initialize
        GameObject arrow = Instantiate(firearrowObj, transform.position, transform.rotation);
        Rigidbody2D arrowrigid = arrow.GetComponent<Rigidbody2D>();
        SpriteRenderer arrowren = arrow.GetComponent<SpriteRenderer>();

        // shoot to player facing
        if (shootDirection < 0)
        {
            arrowren.flipX = true;
        }
        arrowrigid.AddForce(Vector2.right * 10 * shootDirection, ForceMode2D.Impulse);

        // sound
        audiosrc.clip = AudioManager.Instance.playerSkill;
        audiosrc.Play();

        Invoke("availableControl", 0.2f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // normal enemy collision
        if(collision.gameObject.layer == 11)
        {
            onDamaged(collision.transform.position, 1);
        }

        // boss attack collision
        else if(collision.gameObject.layer == 14)
        {
            // fireball hit
            if (collision.gameObject.name.Contains("Fireball"))
            {
                onDamaged(collision.transform.position, 1);
            }

            // bite hit
            else if (collision.gameObject.name.Contains("Bite"))
            {
                onDamaged(collision.transform.position, 2);
            }

            // deathbreath hit
            else if (collision.gameObject.name.Contains("DeathBreath"))
            {
                onDamaged(collision.transform.position, 3);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            // Coin: Get Point
            if (collision.gameObject.name.Contains("Coin"))
            {
                GameManager.Instance.stagePoint += 50;
                // sound
                audiosrc.clip = AudioManager.Instance.playerCoin;
                audiosrc.Play();
            }

            // Heart: Get Health
            else if (collision.gameObject.name.Contains("Heart"))
            {
                if (GameManager.Instance.curHP < GameManager.Instance.maxHP)
                {
                    GameManager.Instance.curHP += 1;
                }
                // sound
                audiosrc.clip = AudioManager.Instance.playerHeart;
                audiosrc.Play();
            }


            // Item inactive
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Finish")
        {
            // sound
            audiosrc.clip = AudioManager.Instance.playerClear;
            audiosrc.Play();

            // disable control for a while
            disableControl();
            Invoke("availableControl", 3f);

            // Next Stage
            GameManager.Instance.PlayerClear();

        }
        else if (collision.gameObject.tag == "Border")
        {
            // disable control for a while
            disableControl();

            // call die func
            Die();
            return;
        }
    }

    void onDamaged(Vector2 targetPos, int dmg)
    {
        // immortal active
        gameObject.layer = 10;

        // disable control for a while
        disableControl();

        // health calculation
        GameManager.Instance.curHP -= dmg;
        if (GameManager.Instance.curHP <= 0)
        {
            // view normal
            ren.color = new Color(1, 1, 1, 1);

            // call die func
            Die();
            return;
        }

        // sound
        audiosrc.clip = AudioManager.Instance.playerHit;
        audiosrc.Play();

        // view alpha
        ren.color = new Color(1f, 0.1f, 0.1f, 1);

        // damaged reaction
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc * 50, 25), ForceMode2D.Impulse);

        // go to normal state
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

    public void availableControl()
    {
        controlDisabled = false;
        GameManager.Instance.enableAction = true;
    }

    public void disableControl()
    {
        controlDisabled = true;
        GameManager.Instance.enableAction = false;
    }

    void Die()
    {
        // stop move
        CancelInvoke();
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        
        // let gamemanager know
        GameManager.Instance.PlayerDie();

        // dying anim
        anim.SetTrigger("Die");
    }

    public void ButtonDown(string type)
    {
        switch (type)
        {
            case "L":
                left_value = -1;
                break;
            case "R":
                right_value = 1;
                break;
            case "J":
                jump_value = true;
                break;
            case "A":
                attack_value = true;
                break;
            case "S":
                skill_value = true;
                break;
        }
    }

    public void ButtonUp(string type)
    {
        switch (type)
        {
            case "L":
                left_value = 0;
                break;
            case "R":
                right_value = 0;
                break;
            case "J":
                jump_value = false;
                break;
            case "A":
                attack_value = false;
                break;
            case "S":
                skill_value = false;
                break;
        }
    }

}