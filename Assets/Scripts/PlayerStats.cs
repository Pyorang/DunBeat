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

    private PlayerAnim Base;
    private PlayerAnim Hair;

    void Start()
    {
        Base = GameObject.FindWithTag("PlayerBody").GetComponent<PlayerAnim>();
        Hair = GameObject.FindWithTag("PlayerHair").GetComponent<PlayerAnim>();
    }

    public void Hurt()
    {
        isInvincible = true;
        playerHP--;
        Base.isHurting = true;
        Hair.isHurting = true;
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
