using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float MoveSpeed;    //이동속도

    public float RollingCoolTime; //구르기 쿨타임
    public bool isRolling;       //구르기 여부
    public bool isMoving;       //움직이고 있는지
    public bool isFlipped;       //좌우반전인지
    public bool canRolling;      //구르기 가능한지

    //컴포넌트
    [SerializeField]
    PlayerStats stats;

    private Rigidbody2D rb;

    void Start()
    {
        isRolling = false;
        isMoving = false;
        isFlipped = false;
        canRolling = true;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Roll();
        CheckMoving();
    }

    void Move()
    {
        if(!isRolling)
            rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized * MoveSpeed;
        if(rb.velocity != Vector2.zero)
            isMoving = true;
        else isMoving = false;
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
            rb.velocity = 2  * rb.velocity;
            stats.Rollingincible();
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

    void CheckMoving()
    {
        if(rb.velocity == Vector2.zero)
        {
            isMoving = false;
        }
    }
}
