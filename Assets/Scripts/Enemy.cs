using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected string monsterName;
    [SerializeField]
    protected int maxHP;          //최대 HP
    [SerializeField]
    protected float eyeDistance;    //시야
    [SerializeField]
    protected float enemySpeed;  //적 이동 속도
    [SerializeField]
    protected float attackRange;      //공격 범위
    [SerializeField]
    protected int currentHP;      //현재 HP
    [SerializeField]
    protected float dieAnimTime;    //죽는데 걸리는 시간

    protected bool isDead;      //죽었는지
    protected bool isDamaged;     //플레이어에게 데미지를 입은 경우
    protected bool isAttacking;     //공격하고 있는 경우
    protected bool isMoving;        //이동하고 있는 경우
    protected bool hasDetected;   //플레이어를 탐지한 경우
    protected bool startBattle;   //배틀 시작
    protected bool isBattle;      //전투모드에 들어간 경우
    protected int patternNumb;    //현재 패턴 번호

    //필요한 컴포넌트
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Animator anim;
    protected GameObject player;

    void Start()
    {
        patternNumb = 0;
        currentHP = maxHP;
        isDead = false;
        isDamaged = false;
        hasDetected = false;
        isBattle = false;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (player != null)
        {
            BeforeBattle();
            FlipEnemy();
        }
        else
        {
            anim.SetBool("BattleStart", false);
        }
    }

    public string GetName()
    {
        return monsterName;
    }

    //전투 시작전 -> 시작 후로 바꿔주기
    protected void BeforeBattle()
    {
        if (!isBattle)
        {
            Detecting();
            if (isBattle || isDamaged)
            {
                isBattle = true;
                anim.SetBool("BattleStart",true);
                StartCoroutine("Battle");
            }
        }
    }

    //적을 찾을 때
    protected void Detecting()
    {
        if (!hasDetected)
        {
            float playerDistance = Vector2.Distance(transform.position, player.transform.position);
            if (playerDistance < eyeDistance)
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
            rb.velocity = (player.transform.position - transform.position).normalized * enemySpeed;
        }
    }

    //공격 함수
    protected void Attack()
    {
        isAttacking = true;
        anim.SetTrigger("isAttack");

        if (Vector2.Distance(player.transform.position, transform.position) <= attackRange)
        {
            player.GetComponent<PlayerStats>().Hurt();
        }
    }

    //데미지 적용 함수
    public void Damaged(GameObject gameobject, int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
            Die();
        else
        {
            isDamaged = true;
            if (!(isAttacking || isMoving))
                anim.SetTrigger("isHurt");
            else StartCoroutine("HurtColorChange");
        }
    }

    //적 죽음 처리
    protected void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        anim.SetTrigger("isDead");

        StopCoroutine("Battle");
        StartCoroutine("DestroyEnemy");
    }

    protected void FlipEnemy()
    {
        if (player.transform.position.x <= transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;
    }

    protected virtual IEnumerator Battle()
    {
        yield return null;
    }

    public int GetMaxHp() { return maxHP; }
    public int GetCurHp() { return currentHP; }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(dieAnimTime);
        Destroy(gameObject);
        GameManager.instance.currentEnemyCount--;
        if (GameManager.instance.currentEnemyCount == 0)
            GameManager.instance.goNextWave = true;
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
