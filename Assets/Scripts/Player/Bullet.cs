using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        transform.eulerAngles = new Vector3(0, 0, GetAngle(transform.position, GameManager.instance.currentEnemy.transform.position));
        rb.velocity = (GameManager.instance.currentEnemy.transform.position- transform.position).normalized * bulletSpeed;
        StartCoroutine(DestroyNoHitBullet());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Damaged(gameObject, GameManager.instance.currentPlayerAp * GameManager.instance.currentTimingBonus);
            //적 피통 UI 바꾸기
            GameManager.instance.enemyStaus.ChangeEnemy(other.gameObject);
            GameManager.instance.enemyStaus.ShowStatus();
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Bullet Hit");
            StartCoroutine(DestroyHitBullet());
        }
    }

    public float GetAngle(Vector2 start, Vector2 end)
    {
        Vector2 v2 = end - start;
        return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
    }

    IEnumerator DestroyHitBullet()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    IEnumerator DestroyNoHitBullet()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
