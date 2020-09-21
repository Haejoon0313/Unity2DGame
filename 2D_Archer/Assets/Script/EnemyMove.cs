using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    float maxSpeed = 12;
    float speedBoost = -350;
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
        rigid.velocity = new Vector2(speedBoost * Time.deltaTime, rigid.velocity.y);

        if (rigid.velocity.x > maxSpeed) // Right Max
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        if (rigid.velocity.x < maxSpeed*(-1)) // Left Max
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);

        //  Direction Sprite
        anim.SetFloat("Facing", rigid.velocity.x);

        // Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + (speedBoost / 250f), rigid.position.y);
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down * 2,2, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    void Turn()
    {
        speedBoost *= -1;
    }
}
