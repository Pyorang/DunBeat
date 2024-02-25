using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float RollingCoolTime; //구르기 쿨타임
    public bool isRolling;       //구르기 여부
    public bool isMoving;       //움직이고 있는지
    public bool isFlipped;       //좌우반전인지
    public bool canRolling;      //구르기 가능한지

    //컴포넌트
    [SerializeField]
    private PlayerStats stats;
    [SerializeField]
    private UI_EnemyStatus enemyStatus;
    [SerializeField]
    private Ui_ComboControl comboControl;
    [SerializeField]
    private TimingManager timingManager;
    [SerializeField]
    private LayerMask targetMask;       //적 타겟 마스크

    private Rigidbody2D rb;

    private GameObject Base;
    private GameObject Hair;

    [SerializeField]
    private GameObject bulletPrefab;

    void Start()
    {
        isRolling = false;
        isMoving = false;
        isFlipped = false;
        canRolling = true;
        rb = GetComponent<Rigidbody2D>();

        Base = GameObject.FindWithTag("PlayerBody");
        Hair = GameObject.FindWithTag("PlayerHair");
    }

    void Update()
    {
        GameManager.instance.playerPosition = gameObject.transform.position;
        if(!stats.isDead)
        {
            Move();
            ChangeFlip();

            if(!stats.isHurting)
            {
                Roll();
                Attack();
            }
        }
    }

    //플레이어 좌우 향하기
    void ChangeFlip()
    {
        if (isMoving)
        {
            if (isFlipped == true)
            {
                Base.GetComponent<SpriteRenderer>().flipX = true;
                Hair.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                Base.GetComponent<SpriteRenderer>().flipX = false;
                Hair.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    //플레이어 이동
    void Move()
    {
        if(!isRolling)
            rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized * GameManager.instance.playerMoveSpeed;

        if (rb.velocity != Vector2.zero)
        {
            isMoving = true;
            Base.GetComponent<Animator>().SetBool("isMove", true);
            Hair.GetComponent<Animator>().SetBool("isMove", true);
        }
        else
        {
            isMoving = false;
            Base.GetComponent<Animator>().SetBool("isMove", false);
            Hair.GetComponent<Animator>().SetBool("isMove", false);
        }

        if (rb.velocity.x < 0)
            isFlipped = true;
        else if (rb.velocity.x >0)
            isFlipped = false;

    }

    void Roll()
    {
        if (isMoving && canRolling && Input.GetKeyDown(KeyCode.Space))
        {
            isRolling = true;
            canRolling = false;
            Base.GetComponent<Animator>().SetTrigger("isRoll");
            Hair.GetComponent<Animator>().SetTrigger("isRoll");
            rb.velocity = 2  * rb.velocity;
            stats.RollingIncible();
            Invoke("Rolling", 1);
            Invoke("RollCoolTime", RollingCoolTime);
        }
    }

    void Rolling()
    {
        rb.velocity = Vector2.zero;
        isRolling = false;
    }

    void RollCoolTime()
    {
        canRolling = true;
    }

    void Attack()
    {
        if(!isRolling)
        {
            GameObject enemy = FindNearestEnemy();
            GameManager.instance.currentEnemy = enemy;
            if (enemy != null && enemy.GetComponent<Enemy>().GetCurHp() > 0)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if(timingManager.CheckTiming() != timingManager.GetBoxsLength())
                    {
                        SoundManager.instance.PlaySE("Tap");
                        Base.GetComponent<Animator>().SetTrigger("isAttack");
                        Hair.GetComponent<Animator>().SetTrigger("isAttack");
                        Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.identity);
                        comboControl.ComboSuccess();
                    }
                    else
                    {
                        comboControl.ComboFail();
                    }
                }
            }
        }
    }

    //가장 가까운 적 찾기
    GameObject FindNearestEnemy()
    {
        Collider2D[] Enemys = Physics2D.OverlapCircleAll(transform.position, GameManager.instance.playerAttackRange, targetMask);

        if (Enemys.Length != 0)
        {
            Collider2D enemy = Enemys[0];
            float shortDis = Vector2.Distance(transform.position, enemy.transform.position);

            foreach (Collider2D found in Enemys)
            {
                float Distance = Vector2.Distance(transform.position, found.transform.position);

                if (Distance < shortDis)
                {
                    enemy = found;
                    shortDis = Distance;
                }
            }

            return enemy.gameObject;
        }

        else return null;
    }
}
