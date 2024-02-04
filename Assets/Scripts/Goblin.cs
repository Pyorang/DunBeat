using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goblin : Enemy
{
    protected override IEnumerator Battle()
    {
        if(!Player.GetComponent<PlayerStats>().isDead || !isDead)
        {
            if (PatternNumb != 3)
            {
                Moving();
                yield return new WaitForSeconds(0.5f);
                isMoving = false;
                rb.velocity = Vector3.zero;
                yield return new WaitForSeconds(2 - 0.5f);
            }

            else
            {
                attack();
                yield return new WaitForSeconds(2);
                isAttacking = false;
            }

            PatternNumb++;
            if(PatternNumb > 3) {PatternNumb = 0;}
            StartCoroutine("Battle");
        }
    }
}
