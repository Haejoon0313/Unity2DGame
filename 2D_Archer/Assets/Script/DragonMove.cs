using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMove : MonoBehaviour
{
    // Enemy Variables
    int attackId = 0;
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

    // Attack 3 Variables
    public float curUltTime = 0;
    float maxUltTime = 10;
    public GameObject deathbreathObj;

    // Environment
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer ren;
    public GameObject player;

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
        curUltTime += Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            if (!anim.GetBool("isBattle"))
            {
                CancelInvoke();
                anim.SetTrigger("Hurt");
                Invoke("Think", 2.5f);
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
            Invoke("Think", 2.5f);
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
        // i, j for random range
        int i = 1;
        int j = 2;

        // if player too close, bite attack addition
        if (player.transform.position.x > 20)
        {
            i = 0;
        }

        if(curUltTime > maxUltTime)
        {
            j = 3;
        }

        // select attack randomly
        attackId = Random.Range(i, j);

        switch (attackId)
        {
            // case 0: close attack bite
            case 0:
                anim.SetTrigger("Bite");
                Invoke("Bite", 2.5f);
                break;
            // case 1: summon fireball
            case 1:
                anim.SetTrigger("SummonFire");
                SummonCircle(5);
                break;
            // case 2: ultimate attack
            case 2:
                // cooltime start
                curUltTime = 0;
                anim.SetTrigger("DeathBreath");
                Invoke("DeathBreath", 4.5f);
                break;

        }
    }

    void SummonCircle(int num)
    {
        
        for(int i = 0; i < num; i++)
        {
            summonXpos.Add(Random.Range(-2.5f, 20f));
        }

        foreach (float x in summonXpos)
        {
            summonCircle.Add(Instantiate(circleObj, new Vector3(x, 0.7f, 0), Quaternion.Euler(0, 0, 0)));
        }

        // after 1.2s, summon fire
        Invoke("SummonFire", 1.2f);
    }
    void SummonFire()
    {
        foreach (float x in summonXpos)
        {
            summonFireball.Add(Instantiate(fireballObj, new Vector3(x, 0, 0), Quaternion.Euler(0, 0, 180)));
        }

        foreach (GameObject f in summonFireball)
        {
            Rigidbody2D fireballrigid = f.GetComponent<Rigidbody2D>();
            fireballrigid.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
        }

        // SkillEnd
        Invoke("SkillEnd", 2.5f);

        // next attack think
        Invoke("Think", 3.8f);
    }

    
    void Bite()
    {
        // collider active
        biteObj.SetActive(true);

        // SkillEnd
        Invoke("SkillEnd", 1f);

        // next attack think
        Invoke("Think", 2.5f);
    }

    void DeathBreath()
    {
        // collider active
        deathbreathObj.SetActive(true);

        // SkillEnd
        Invoke("SkillEnd", 2f);

        // next attack think
        Invoke("Think", 4f);
    }

    void SkillEnd()
    {
        foreach (GameObject c in summonCircle)
        {
            Destroy(c);
        }

        foreach (GameObject f in summonFireball)
        {
            Destroy(f);
        }

        summonXpos.Clear();
        summonCircle.Clear();
        summonFireball.Clear();
        biteObj.SetActive(false);
        deathbreathObj.SetActive(false);
    }

    void Die()
    {
        GameManager.Instance.stagePoint += 1000;

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
        ren.color = new Color(0.5f, 0.5f, 0.5f, 0.7f);
    }
}
