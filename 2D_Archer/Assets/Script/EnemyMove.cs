using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    float maxSpeed = 12;
    int speedDirection = 1;
    int ThinkTime = 2;

    Rigidbody2D rigid;
    Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        Invoke("Think", ThinkTime);
    }

    // Update is called once per frame
    void Update()
    {

        // Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x* 0.2f, rigid.velocity.y);
        }

        // Player Moving
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

        if (rigid.velocity.x > maxSpeed) // Right Max
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        if (rigid.velocity.x < maxSpeed * (-1)) // Left Max
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

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
        if(collision.gameObject.tag == "arrow")
        {

        }
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
