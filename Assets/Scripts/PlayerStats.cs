using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public bool isInvincible;

    [SerializeField]
    private int playerHP;
    [SerializeField]
    private int playerAP;

    public void Hurt()
    {
        isInvincible = true;
        playerHP--;
        StartCoroutine("invincibleOff");
    }

    public void Rollingincible()
    {
        isInvincible = true;
        StartCoroutine("invincibleOff");
    }

    IEnumerator invincibleOff()
    {
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }

}
