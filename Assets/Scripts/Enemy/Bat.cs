using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    [SerializeField]
    private GameObject laserPrefab;

    protected override IEnumerator Battle()
    {
        if (!player.GetComponent<PlayerStats>().isDead || !isDead)
        {
            if (patternNumb != 1)
            {
                Moving();
                yield return new WaitForSeconds(0.5f);
                isMoving = false;
                rb.velocity = Vector3.zero;
                yield return new WaitForSeconds(2 - 0.5f);
            }

            else
            {
                Attack();
                yield return new WaitForSeconds(2);
                isAttacking = false;
            }

            patternNumb++;
            if (patternNumb > 1) { patternNumb = 0; }
            StartCoroutine(Battle());
        }
    }

    //���� �Լ�
    protected override void Attack()
    {
        isAttacking = true;

        if (Vector2.Distance(player.transform.position, transform.position) <= attackRange)
        {
            Instantiate(laserPrefab, gameObject.transform.position, Quaternion.identity);
        }
    }

    protected override void Die()
    {
        isDead = true;
        anim.SetTrigger("isHurt");
        StopCoroutine(Battle());

        rb.velocity = Vector2.zero;

        StartCoroutine(DestroyEnemy());
    }

    //���� ������ -> ���� �ķ� �ٲ��ֱ�
    protected override void BeforeBattle()
    {
        if (!isBattle)
        {
            Detecting();
            if (isBattle || isDamaged)
            {
                isBattle = true;
                StartCoroutine(Battle());
            }
        }
    }
}
