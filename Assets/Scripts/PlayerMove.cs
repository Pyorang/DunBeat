using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;    //이동속도
    [SerializeField]
    private float attackRange;      //공격 사거리

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

    private Rigidbody2D rb;

    private GameObject Base;
    private GameObject Hair;

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

    // Update is called once per frame
    void Update()
    {
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
            rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized * moveSpeed;

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
            if (enemy != null)
            {
                if (Vector2.Distance(gameObject.transform.position, enemy.transform.position) <= attackRange && enemy.GetComponent<Enemy>().GetCurHp()>0)
                {
                    if (Input.GetKeyDown(KeyCode.Z))     //리듬 성공할 경우 추가할 것
                    {
                        Base.GetComponent<Animator>().SetTrigger("isAttack");
                        Hair.GetComponent<Animator>().SetTrigger("isAttack");
                        enemy.GetComponent<Enemy>().Damaged(gameObject, stats.playerAP);

                        enemyStatus.ChangeEnemy(enemy);
                        enemyStatus.ShowStatus();

                        comboControl.ComboSuccess();
                    }
                }
            }
        }
    }

    //가장 가까운 적 찾기
    GameObject FindNearestEnemy()
    {
        List<GameObject> FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        if (FoundObjects.Count > 0)
        {
            GameObject enemy = FoundObjects[0];
            float shortDis = Vector2.Distance(gameObject.transform.position, enemy.transform.position);

            foreach (GameObject found in FoundObjects)
            {
                float Distance = Vector2.Distance(gameObject.transform.position, found.transform.position);

                if (Distance < shortDis)
                {
                    enemy = found;
                    shortDis = Distance;
                }
            }

            return enemy;
        }

        else return null;
    }
}
