using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemArm : MonoBehaviour
{
    [SerializeField]
    private float armSpeed;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        StartCoroutine(DestroyNoHitBullet());
    }

    void Update()
    {
        rb.velocity = (GameManager.instance.playerPosition - transform.position).normalized * armSpeed;
        transform.eulerAngles = new Vector3(0, 0, GetAngle(transform.position, GameManager.instance.playerPosition));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().Hurt();
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
        Destroy(gameObject);
        yield return null;
    }

    IEnumerator DestroyNoHitBullet()
    {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
}
