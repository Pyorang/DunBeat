using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    private bool isLowHP;

    [SerializeField]
    private float shootingLaserTime;    //������ ��� �ð�
    [SerializeField]
    private float isImmuneTime;     //�鿪 ����
    [SerializeField]
    private float shootingLaserRange;   //������ ���� (���� ����)

    [SerializeField]
    private GameObject golemArmPrefab;
    [SerializeField]
    private GameObject golemLaserPrefab;
    [SerializeField]
    private Vector3 golemArmSpawnPosition;
    [SerializeField]
    private Vector3 golemLaserSpawnPosition;

    protected override void LowHP() { isLowHP = true; }

    //�� ������ ����
    protected override void Attack()
    {
        isAttacking = true;
        anim.SetTrigger("isAttack");
        if (sr.flipX == true)
            Instantiate(golemArmPrefab, gameObject.transform.position - golemArmSpawnPosition, Quaternion.Euler(0, 180, 0));
        else
            Instantiate(golemArmPrefab, gameObject.transform.position + golemArmSpawnPosition, Quaternion.identity);
    }

    private void LaserAttack()
    {
        isAttacking = true;
        anim.SetTrigger("isLaserAttack");
        if (sr.flipX == true)
            Instantiate(golemLaserPrefab, gameObject.transform.position + golemLaserSpawnPosition - new Vector3(2 * golemLaserSpawnPosition.x,0,0), Quaternion.identity);
        else
            Instantiate(golemLaserPrefab, gameObject.transform.position + golemLaserSpawnPosition, Quaternion.identity);
    }

    protected override IEnumerator Battle()
    {
        if (!player.GetComponent<PlayerStats>().isDead || !isDead)
        {
            if(isLowHP)
            {
                if (patternNumb == 0)       //�̵�
                {
                    Moving();
                    yield return new WaitForSeconds(0.5f);
                    isMoving = false;
                    rb.velocity = Vector3.zero;
                    yield return new WaitForSeconds(2 - 0.5f);
                }

                else if(patternNumb == 1)   //��� ����
                {
                    isImmune = true;
                    anim.SetBool("isImmune", true);
                    sr.color = new Color(232 / 255f, 105 / 255f, 255 / 255f, 255 / 255f);
                    yield return new WaitForSeconds(isImmuneTime);
                    StartCoroutine(ImmuneOver());
                }

                else if (patternNumb == 2)   //����
                {
                    if (Vector2.Distance(player.transform.position, transform.position) > shootingLaserRange && Vector2.Distance(player.transform.position, transform.position) >= attackRange)
                    {
                        yield return new WaitForSeconds(1f);
                        Attack();
                        yield return new WaitForSeconds(1f);
                        isAttacking = false;
                    }
                    else if (Vector2.Distance(player.transform.position, transform.position) <= shootingLaserRange)
                    {
                        yield return new WaitForSeconds(1f);
                        LaserAttack();
                        yield return new WaitForSeconds(shootingLaserTime);
                        isAttacking = false;
                    }
                }

                patternNumb++;
                if (patternNumb > 2) { patternNumb = 0; }
                StartCoroutine(Battle());
            }
            else
            {
                if (patternNumb == 0)       //�̵� ����
                {
                    Moving();
                    yield return new WaitForSeconds(0.5f);
                    isMoving = false;
                    rb.velocity = Vector3.zero;
                    yield return new WaitForSeconds(2 - 0.5f);
                }

                else if(patternNumb != 0)   //���� ����
                {
                    if (Vector2.Distance(player.transform.position, transform.position) > shootingLaserRange && Vector2.Distance(player.transform.position, transform.position) <= attackRange)
                    {
                        yield return new WaitForSeconds(1f);
                        Attack();
                        yield return new WaitForSeconds(1f);
                        isAttacking = false;
                    }
                    else if (Vector2.Distance(player.transform.position, transform.position) <= shootingLaserRange)
                    {
                        yield return new WaitForSeconds(1f);
                        LaserAttack();
                        yield return new WaitForSeconds(shootingLaserTime);
                        isAttacking = false;
                    }
                }

                patternNumb++;
                patternNumb++;
                if (patternNumb > 3) { patternNumb = 0; }
                StartCoroutine(Battle());
            }
        }
    }

    private IEnumerator ImmuneOver()
    {
        sr.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
        isImmune = false;
        anim.SetBool("isImmune", false);
        yield return null;
    }
}
