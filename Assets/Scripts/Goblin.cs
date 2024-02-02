using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    [SerializeField]
    private int maxHP;          //최대 HP
    [SerializeField]
    private float EyeDistance;    //고블린 시야
    [SerializeField]
    private float GoblinSpeed;  //고블린 이동 속도
    [SerializeField]
    private float knockbackPower;   //넉백 정도
    [SerializeField]
    private float attackRange;      //공격 범위

    private int currentHP;      //현재 HP
    private bool isDamaged;     //플레이어에게 데미지를 입은 경우
    private bool hasDetected;   //플레이어를 탐지한 경우
    private bool startBattle;   //배틀 시작
    private bool isBattle;      //전투모드에 들어간 경우

    //필요한 컴포넌트
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private GameObject Player;

    void Start()
    {
        currentHP = maxHP;
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
        BeforeBattle();
        flipGoblin();
    }

    //고블린 전투 시작전 -> 시작 후로 바꿔주기
    void BeforeBattle()
    {
        if (!isBattle || isDamaged)
        {
            detecting();
            if (isBattle)
            {
                anim.SetTrigger("BattleStart");
                StartCoroutine("Battle");
            }
        }
    }

    //고블린이 적을 찾을 때
    void detecting()
    {
        if(!hasDetected)
        {
            float PlayerDistance = Vector2.Distance(transform.position, Player.transform.position);
            if (PlayerDistance < EyeDistance)
            {
                hasDetected = true;
                isBattle = true;
            }
        }
    }

    //고블린 이동 함수 
    void jump()
    {
        if (isBattle)
        {
            anim.SetBool("isAttack", false);
            rb.velocity = (Player.transform.position - transform.position).normalized* GoblinSpeed;
        }
    }

    //넉백
    void knockback(GameObject gameobject)
    {
        if(isBattle)
        {
            rb.velocity = (gameobject.transform.position - transform.position).normalized * knockbackPower;
        }
    }

    //공격 함수
    void attack()
    {
        anim.SetBool("isAttack", true);

        if(Vector2.Distance(Player.transform.position, transform.position) <= attackRange)
        {
            Player.GetComponent<PlayerStats>().Hurt();
        }
    }

    //고블린 데미지 적용 함수
    void damaged(GameObject gameobject, int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
            die();
        isDamaged = true;
        isBattle = true;
        knockback(gameobject);
        die();
    }

    //고블린 죽음 처리
    void die()
    {
        if(currentHP<0)
        {
            //고블린 파괴
        }
    }

    void flipGoblin()
    {
        if (Player.transform.position.x <= transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;
    }

   IEnumerator Battle()
    {
        jump();
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(2 - 0.5f);
        jump();
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(2 - 0.5f);
        jump();
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.5f);
        attack();
        yield return new WaitForSeconds(2);
        StartCoroutine("Battle");
    }
}
