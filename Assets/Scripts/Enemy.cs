using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected int maxHP;          //�ִ� HP
    [SerializeField]
    protected float EyeDistance;    //�þ�
    [SerializeField]
    protected float EnemySpeed;  //�� �̵� �ӵ�
    [SerializeField]
    protected float attackRange;      //���� ����
    [SerializeField]
    protected int currentHP;      //���� HP
    [SerializeField]
    protected float DieAnimTime;    //�״µ� �ɸ��� �ð�

    protected bool isDead;      //�׾�����
    protected bool isDamaged;     //�÷��̾�� �������� ���� ���
    protected bool isAttacking;     //�����ϰ� �ִ� ���
    protected bool isMoving;        //�̵��ϰ� �ִ� ���
    protected bool hasDetected;   //�÷��̾ Ž���� ���
    protected bool startBattle;   //��Ʋ ����
    protected bool isBattle;      //������忡 �� ���
    protected int PatternNumb;    //���� ���� ��ȣ

    //�ʿ��� ������Ʈ
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

    //���� ������ -> ���� �ķ� �ٲ��ֱ�
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

    //���� ã�� ��
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

    //�̵�
    protected void Moving()
    {
        if (isBattle)
        {
            isMoving = true;
            rb.velocity = (Player.transform.position - transform.position).normalized * EnemySpeed;
        }
    }

    //���� �Լ�
    protected void attack()
    {
        isAttacking = true;
        anim.SetTrigger("isAttack");

        if (Vector2.Distance(Player.transform.position, transform.position) <= attackRange)
        {
            Player.GetComponent<PlayerStats>().Hurt();
        }
    }

    //������ ���� �Լ�
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

    //�� ���� ó��
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
