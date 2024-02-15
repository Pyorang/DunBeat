using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goblin : Enemy
{
    protected override IEnumerator Battle()
    {
        if(!player.GetComponent<PlayerStats>().isDead || !isDead)
        {
            if (patternNumb != 3)
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
            if(patternNumb > 3) {patternNumb = 0;}
            StartCoroutine(Battle());
        }
    }
}
