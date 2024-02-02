using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    [SerializeField]
    private int maxHP;          //�ִ� HP
    [SerializeField]
    private float EyeDistance;    //��� �þ�
    [SerializeField]
    private float GoblinSpeed;  //��� �̵� �ӵ�
    [SerializeField]
    private float knockbackPower;   //�˹� ����
    [SerializeField]
    private float attackRange;      //���� ����

    private int currentHP;      //���� HP
    private bool isDamaged;     //�÷��̾�� �������� ���� ���
    private bool hasDetected;   //�÷��̾ Ž���� ���
    private bool startBattle;   //��Ʋ ����
    private bool isBattle;      //������忡 �� ���

    //�ʿ��� ������Ʈ
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

    //��� ���� ������ -> ���� �ķ� �ٲ��ֱ�
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

    //����� ���� ã�� ��
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

    //��� �̵� �Լ� 
    void jump()
    {
        if (isBattle)
        {
            anim.SetBool("isAttack", false);
            rb.velocity = (Player.transform.position - transform.position).normalized* GoblinSpeed;
        }
    }

    //�˹�
    void knockback(GameObject gameobject)
    {
        if(isBattle)
        {
            rb.velocity = (gameobject.transform.position - transform.position).normalized * knockbackPower;
        }
    }

    //���� �Լ�
    void attack()
    {
        anim.SetBool("isAttack", true);

        if(Vector2.Distance(Player.transform.position, transform.position) <= attackRange)
        {
            Player.GetComponent<PlayerStats>().Hurt();
        }
    }

    //��� ������ ���� �Լ�
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

    //��� ���� ó��
    void die()
    {
        if(currentHP<0)
        {
            //��� �ı�
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
