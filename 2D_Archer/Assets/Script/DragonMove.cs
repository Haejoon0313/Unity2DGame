using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMove : MonoBehaviour
{
    // Enemy Variables
    int attackId = 0;

    // Attack 1 Variables
    public GameObject fireballObj;
    public GameObject circleObj;
    List<float> summonXpos = new List<float>();
    List<GameObject> summonCircle = new List<GameObject>();
    List<GameObject> summonFireball = new List<GameObject>();

    // Attack 2 Variables
    public GameObject biteObj;

    // Attack 3 Variables
    public GameObject deathbreathObj;
    public float curUltTime = 0;
    float maxUltTime = 10;
    bool isBreathing = false;

    // Environment
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer ren;
    AudioSource audiosrc;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ren = GetComponent<SpriteRenderer>();
        audiosrc = GetComponent<AudioSource>();

        GameManager.Instance.curBossHP = GameManager.Instance.maxBossHP;
    }

    void Update()
    {
        if(GameManager.Instance.curBossHP != GameManager.Instance.maxBossHP)
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
                AudioManager.Instance.ChangeBGM(1);
                Invoke("Think", 2.5f);
            }
            onDamaged(1);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isBreathing && collision.gameObject.tag == "Ultimate")
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
        GameManager.Instance.curBossHP -= dmg;

        if (GameManager.Instance.curBossHP <= 0)
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
        if (PlayerMove.Instance.gameObject.transform.position.x > 20)
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
                audiosrc.clip = AudioManager.Instance.bossBreath;
                audiosrc.Play();
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
        // sound
        audiosrc.clip = AudioManager.Instance.bossLeg;
        audiosrc.Play();

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
        // sound
        audiosrc.clip = AudioManager.Instance.bossBreath;
        audiosrc.Play();

        // collider active
        deathbreathObj.SetActive(true);

        // after start breathing, don't hurt
        isBreathing = true;

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

        audiosrc.Stop();

        summonXpos.Clear();
        summonCircle.Clear();
        summonFireball.Clear();
        biteObj.SetActive(false);
        isBreathing = false;
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
        SkillEnd();

        // dying anim
        anim.SetTrigger("Die");

        Invoke("DeadBody", 6.5f);
    }

    void DeadBody()
    {
        ren.color = new Color(0.5f, 0.5f, 0.5f, 0.7f);
    }
}
