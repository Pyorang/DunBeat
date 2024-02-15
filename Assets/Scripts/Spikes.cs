using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private bool isAttacking;
    private bool onSpike;

    [SerializeField]
    private float NtoR;
    [SerializeField]
    private float RtoA1;
    [SerializeField]
    private float A1toA2;
    [SerializeField]
    private float A2toN;


    //음악마다 리듬이 달라 sprite 변경시간을 달리하기 위하여
    //4가지로 나누어 놨음

    [SerializeField]
    private Sprite spike_None;
    [SerializeField]
    private Sprite spike_Ready;
    [SerializeField]
    private Sprite spike_Attack1;
    [SerializeField]
    private Sprite spike_Attack2;

    //필요한 컴포넌트
    private SpriteRenderer spriteRenderer;
    private PlayerStats PlayerStats;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        isAttacking = false;
        StartCoroutine(SCnTOr());
    }

    void Update()
    {
        if (onSpike)
            Damage();
    }


    IEnumerator SCnTOr()
    {
        yield return new WaitForSeconds(NtoR);
        spriteRenderer.sprite = spike_Ready;
        StartCoroutine("SCrTOa1");
    }

    IEnumerator SCrTOa1()
    {
        yield return new WaitForSeconds(RtoA1);
        spriteRenderer.sprite = spike_Attack1;
        StartCoroutine("SCa1TOa2");
    }

    IEnumerator SCa1TOa2()
    {
        yield return new WaitForSeconds(A1toA2);
        spriteRenderer.sprite = spike_Attack2;
        isAttacking = true;
        StartCoroutine("SCa2TOn");
    }

    IEnumerator SCa2TOn()
    {
        yield return new WaitForSeconds(A2toN);
        spriteRenderer.sprite = spike_None;
        isAttacking = false;
        StartCoroutine("SCnTOr");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            onSpike = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            onSpike = false;
    }

    void Damage()
    {
        if (isAttacking && !PlayerStats.isInvincible)
        {
            PlayerStats.Hurt();
        }
    }
}
