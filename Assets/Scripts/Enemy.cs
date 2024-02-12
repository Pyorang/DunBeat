using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected string monsterName;
    [SerializeField]
    protected int maxHP;          //�ִ� HP
    [SerializeField]
    protected float eyeDistance;    //�þ�
    [SerializeField]
    protected float enemySpeed;  //�� �̵� �ӵ�
    [SerializeField]
    protected float attackRange;      //���� ����
    [SerializeField]
    protected int currentHP;      //���� HP
    [SerializeField]
    protected float dieAnimTime;    //�״µ� �ɸ��� �ð�

    protected bool isDead;      //�׾�����
    protected bool isDamaged;     //�÷��̾�� �������� ���� ���
    protected bool isAttacking;     //�����ϰ� �ִ� ���
    protected bool isMoving;        //�̵��ϰ� �ִ� ���
    protected bool hasDetected;   //�÷��̾ Ž���� ���
    protected bool startBattle;   //��Ʋ ����
    protected bool isBattle;      //������忡 �� ���
    protected int patternNumb;    //���� ���� ��ȣ

    //�ʿ��� ������Ʈ
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

    //���� ������ -> ���� �ķ� �ٲ��ֱ�
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

    //���� ã�� ��
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

    //�̵�
    protected void Moving()
    {
        if (isBattle)
        {
            isMoving = true;
            rb.velocity = (player.transform.position - transform.position).normalized * enemySpeed;
        }
    }

    //���� �Լ�
    protected void Attack()
    {
        isAttacking = true;
        anim.SetTrigger("isAttack");

        if (Vector2.Distance(player.transform.position, transform.position) <= attackRange)
        {
            player.GetComponent<PlayerStats>().Hurt();
        }
    }

    //������ ���� �Լ�
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

    //�� ���� ó��
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
