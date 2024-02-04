using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public bool isInvincible;

    public int playerHP;
    public int playerAP;
    public bool isDead;
    public bool isHurting;

    private GameObject Base;
    private GameObject Hair;

    private PlayerMove pm;

    void Start()
    {
        isHurting = false;
        isDead = false;

        Base = GameObject.FindWithTag("PlayerBody");
        Hair = GameObject.FindWithTag("PlayerHair");
        pm = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
    }

    public void Hurt()
    {
        isInvincible = true;
        playerHP--;

        //Á×À½ Ã³¸®
        if (playerHP == 0)
            Die();

        else
        {
            pm.canRolling = false;

            Base.GetComponent<SpriteRenderer>().color = new Color(191 / 255f, 191 / 255f, 191 / 255f, 255 / 255f);
            Hair.GetComponent<SpriteRenderer>().color = new Color(191 / 255f, 191 / 255f, 191 / 255f, 255 / 255f);

            Base.GetComponent<Animator>().SetTrigger("isHurt");
            Hair.GetComponent<Animator>().SetTrigger("isHurt");
        }

        StartCoroutine("invincibleOff");
        StartCoroutine("EndHurtAnim");
    }

    private void Die()
    {
        isDead = true;
        
        pm.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        Base.GetComponent<Animator>().SetTrigger("isDead");
        Hair.GetComponent<Animator>().SetTrigger("isDead");
        
        StartCoroutine("DestroyPlayer");
    }

    public void RollingIncible()
    {
        isInvincible = true;
        StartCoroutine("invincibleOff");
    }

    IEnumerator invincibleOff()
    {
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }

    IEnumerator EndHurtAnim()
    {
        yield return new WaitForSeconds(1f);
        pm.canRolling = true;
        Base.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
        Hair.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
    }

    IEnumerator DestroyPlayer()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}