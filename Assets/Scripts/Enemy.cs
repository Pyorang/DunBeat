using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected int maxHP;          //최대 HP
    [SerializeField]
    protected float EyeDistance;    //시야
    [SerializeField]
    protected float EnemySpeed;  //적 이동 속도
    [SerializeField]
    protected float attackRange;      //공격 범위
    [SerializeField]
    protected int currentHP;      //현재 HP
    [SerializeField]
    protected float DieAnimTime;    //죽는데 걸리는 시간

    protected bool isDead;      //죽었는지
    protected bool isDamaged;     //플레이어에게 데미지를 입은 경우
    protected bool isAttacking;     //공격하고 있는 경우
    protected bool isMoving;        //이동하고 있는 경우
    protected bool hasDetected;   //플레이어를 탐지한 경우
    protected bool startBattle;   //배틀 시작
    protected bool isBattle;      //전투모드에 들어간 경우
    protected int PatternNumb;    //현재 패턴 번호

    //필요한 컴포넌트
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Animator anim;
    protected GameObject Player;

    void Start()
    {
        PatternNumb = 0;
        currentHP = maxHP;
        isDead = false;
        isDamaged = false;
        hasDetected = false;
        isBattle = false;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (Player != null)
        {
            BeforeBattle();
            flipEnemy();
        }
        else
        {
            anim.SetBool("BattleStart", false);
        }
    }

    //전투 시작전 -> 시작 후로 바꿔주기
    protected void BeforeBattle()
    {
        if (!isBattle)
        {
            detecting();
            if (isBattle || isDamaged)
            {
                isBattle = true;
                anim.SetBool("BattleStart",true);
                StartCoroutine("Battle");
            }
        }
    }

    //적을 찾을 때
    protected void detecting()
    {
        if (!hasDetected)
        {
            float PlayerDistance = Vector2.Distance(transform.position, Player.transform.position);
            if (PlayerDistance < EyeDistance)
            {
                hasDetected = true;
                isBattle = true;
            }
        }
    }

    //이동
    protected void Moving()
    {
        if (isBattle)
        {
            isMoving = true;
            rb.velocity = (Player.transform.position - transform.position).normalized * EnemySpeed;
        }
    }

    //공격 함수
    protected void attack()
    {
        isAttacking = true;
        anim.SetTrigger("isAttack");

        if (Vector2.Distance(Player.transform.position, transform.position) <= attackRange)
        {
            Player.GetComponent<PlayerStats>().Hurt();
        }
    }

    //데미지 적용 함수
    public void damaged(GameObject gameobject, int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
            die();
        else
        {
            isDamaged = true;
            if (!(isAttacking || isMoving))
                anim.SetTrigger("isHurt");
            else StartCoroutine("HurtColorChange");
        }
    }

    //적 죽음 처리
    protected void die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        anim.SetTrigger("isDead");

        StopCoroutine("Battle");
        StartCoroutine("DestroyEnemy");
    }

    protected void flipEnemy()
    {
        if (Player.transform.position.x <= transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;
    }

    protected virtual IEnumerator Battle()
    {
        yield return null;
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(DieAnimTime);
        Destroy(gameObject);
    }

    IEnumerator HurtColorChange()
    {
        sr.color = new Color(228/255f, 59/255f, 68/255f, 255/255f);
        yield return new WaitForSeconds(0.1f);

        sr.color = new Color(191 / 255f, 191 / 255f, 191 / 255f, 191 / 255f);
        yield return new WaitForSeconds(0.1f);

        sr.color = new Color(228 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
    }
}
