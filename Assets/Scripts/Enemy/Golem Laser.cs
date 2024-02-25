using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemLaser : MonoBehaviour
{
    private bool canAttack;

    [SerializeField]
    private float laserAnimationTime;
    [SerializeField]
    private float prepareLaserAnimTime;

    void Start()
    {
        canAttack = false;
        StartCoroutine(LaserAttacking());
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, GetAngle(transform.position, GameManager.instance.playerPosition));
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canAttack)
        {
            other.GetComponent<PlayerStats>().Hurt();
        }
    }

    public float GetAngle(Vector2 start, Vector2 end)
    {
        Vector2 v2 = end - start;
        return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
    }

    IEnumerator LaserAttacking()
    {
        yield return new WaitForSeconds(prepareLaserAnimTime);
        canAttack = true;
        yield return new WaitForSeconds(laserAnimationTime);
        Destroy(gameObject);
    }
}
