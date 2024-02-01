using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [SerializeField]
    private float RollingAnimCoolTime;

    public bool Rolling;

    //필요한 컴포넌트
    private Animator anim;
    private SpriteRenderer sr;
    [SerializeField]
    private PlayerMove pm;

    void Start()
    {
        Rolling = false;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        ChangeAnim();
        ChangeFlip();
        ChangeRollAnim();
    }

    void ChangeAnim()
    {
        if(pm.isMoving == true && !Rolling)
        {
            anim.SetBool("isMove", true);
        }
        else
            anim.SetBool("isMove", false);
    }

    void ChangeFlip()
    {
        if(pm.isMoving)
        {
            if (pm.isFlipped == true)
                sr.flipX = true;
            else sr.flipX = false;
        }
    }

    void ChangeRollAnim()
    {
        if(pm.isMoving && !Rolling && pm.isRolling && Input.GetKeyDown(KeyCode.Space))
        {
            Rolling = true;
            anim.SetTrigger("isRoll");
            Invoke("RollAnimCoolTime", RollingAnimCoolTime);
        }
    }

    void RollAnimCoolTime()
    {
        Rolling = false;
        anim.SetTrigger("EndRoll");
    }
}
