using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMove : MonoBehaviour
{
    // Enemy Variables
    int attackId = 0;
    int ThinkTime = 5;
    int maxHP = 100;
    public int health;

    // Attack 1 Variables
    public GameObject fireballObj;
    public GameObject circleObj;
    List<float> summonXpos = new List<float>();
    List<GameObject> summonCircle = new List<GameObject>();
    List<GameObject> summonFireball = new List<GameObject>();

    // Attack 2 Variables
    public GameObject biteObj;

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

        health = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if(health != maxHP)
        {
            anim.SetBool("isBattle", true);
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            if (!anim.GetBool("isBattle"))
            {
                CancelInvoke();
                anim.SetTrigger("Hurt");
                Invoke("Think", ThinkTime / 2);
            }
            onDamaged(1);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ultimate")
        {
            CancelInvoke();
            SkillEnd();
            anim.SetTrigger("Hurt");
            Invoke("Think", ThinkTime / 2);
            onDamaged(10);
        }
    }
    void onDamaged(int dmg)
    {
        // health calculation
        health -= dmg;
        if(health <= 0)
        {
            CancelInvoke();
            Die();
        }
    }

    void Think()
    {
        attackId = Random.Range(1, 3);

        switch (attackId)
        {
            case 1:
                Debug.Log("Attack 1");
                anim.SetTrigger("SummonFire");
                SummonCircle(5);
                break;
            case 2:
                Debug.Log("Attack 2");
                anim.SetTrigger("Bite");
                Invoke("Bite", 2.5f);
                break;
            case 3:
                Debug.Log("Attack 3");
                break;

        }

        Invoke("Think", ThinkTime);
    }

    void SummonCircle(int num)
    {
        
        for(int i = 0; i < num; i++)
        {
            summonXpos.Add(Random.Range(0f, 20f));
        }

        foreach (float x in summonXpos)
        {
            summonCircle.Add(Instantiate(circleObj, new Vector3(x, 0.7f, 0), Quaternion.Euler(0, 0, 0)));
        }

        // after 1.5s, summon fire
        Invoke("SummonFire", 1.5f);
    }
    void SummonFire()
    {
        foreach (float x in summonXpos)
        {
            summonFireball.Add(Instantiate(fireballObj, new Vector3(x, 1.5f, 0), Quaternion.Euler(0, 0, -135)));
        }

        foreach (GameObject f in summonFireball)
        {
            Rigidbody2D fireballrigid = f.GetComponent<Rigidbody2D>();
            fireballrigid.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
        }

        Invoke("SkillEnd", 2.8f);
    }

    
    void Bite()
    {
        biteObj.SetActive(true);

        Invoke("SkillEnd", 1f);
    }

    void SkillEnd()
    {
        foreach (GameObject c in summonCircle)
        {
            Destroy(c);
        }

        summonXpos.Clear();
        summonCircle.Clear();
        summonFireball.Clear();
        biteObj.SetActive(false);
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

        Invoke("DeadBody", 6.5f);
    }

    void DeadBody()
    {
        ren.color = new Color(1, 1, 1, 0.1f*Time.deltaTime);
    }
}
