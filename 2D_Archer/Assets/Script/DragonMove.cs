using UnityEngine;

public class DragonMove : MonoBehaviour
{
    // Enemy Variables
    int attackId = 0;
    int ThinkTime = 2;
    public int health = 100;

    // Environment
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer ren;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ren = GetComponent<SpriteRenderer>();

        Invoke("Think", ThinkTime);
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    void FixedUpdate()
    {

    }

    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            onDamaged(1);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ultimate")
        {
            CancelInvoke();
            anim.SetTrigger("Hurt");
            onDamaged(10);
        }
    }
    void onDamaged(int dmg)
    {
        // health calculation
        int remain = health - dmg;
        if(remain <= 0)
        {

            Die();
        }
        else
        {
            health = remain;
        }

        // go to normal state
        Invoke("Think", ThinkTime);
    }

    void Think()
    {
        attackId = Random.Range(0, 2);

        Invoke("Think", ThinkTime);
    }

    void Die()
    {
        gameManager.stagePoint += 1000;

        // no collision
        gameObject.layer = 10;

        // stop move
        CancelInvoke();
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;

        // dying anim
        anim.SetTrigger("Die");

        Invoke("Delete", 6.5f);
    }

    void Delete()
    {
        gameObject.SetActive(false);
    }
}
