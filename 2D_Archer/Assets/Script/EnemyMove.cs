using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    // Enemy Variables
    int speedDirection = 1;
    int ThinkTime = 2;

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

        // Moving Speed
        rigid.velocity = new Vector2(speedDirection * 250 * Time.deltaTime, rigid.velocity.y);

        //  Direction Sprite
        anim.SetFloat("Facing", rigid.velocity.x);

        // Cliff Check
        Vector2 frontVec = new Vector2(rigid.position.x + speedDirection, rigid.position.y);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down * 2, 2, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Turn();
        }

        // Wall Check
        rayHit = Physics2D.Raycast(rigid.position, Vector3.right * speedDirection, 2, LayerMask.GetMask("Platform"));
        if (rayHit.collider != null)
        {
            Turn();
        }


    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            onDamaged(collision.transform.position);
        }
    }

    void onDamaged(Vector2 targetPos)
    {
        // view alpha
        ren.color = new Color(1f, 0.1f, 0.1f, 1);

        // damaged reaction
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc * 50, 25), ForceMode2D.Impulse);

        speedDirection = dirc*(-1);
        CancelInvoke();
        Invoke("Think", ThinkTime);
        Invoke("offDamaged", 0.5f);

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
;    }
}
