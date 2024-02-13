using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public bool isInvincible;

    public int playerHP;
    public float playerAP;
    public int doorStageNum;
    public int doorWaveNum;
    public bool isDead;
    public bool isHurting;
    private Vector3 goDirection;

    private GameObject Base;
    private GameObject Hair;
    [SerializeField]
    private GameObject playerStatus;

    private PlayerMove pm;
    private Rigidbody2D rb;
    private UI_Health uh;

    void Start()
    {
        isHurting = false;
        isDead = false;
        goDirection = Vector3.right;

        Base = GameObject.FindWithTag("PlayerBody");
        Hair = GameObject.FindWithTag("PlayerHair");
        pm = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();

        rb = GetComponent<Rigidbody2D>();
        uh = playerStatus.GetComponent<UI_Health>();
        GameManager.instance.currentPlayerHealth = playerHP;
    }

    void Update()
    {
        doorStageNum = GameManager.instance.currentStageIndex;
        doorWaveNum = GameManager.instance.currentWaveNum;
        CanOpenDoor();
    }

    public void Hurt()
    {
        isInvincible = true;
        playerHP--;
        GameManager.instance.currentPlayerHealth--;
        uh.ChangeHealthUI();

        //Á×À½ Ã³¸®
        if (playerHP == 0)
            Die();

        else
        {
            if(!isDead)
            {
                pm.canRolling = false;

                Base.GetComponent<SpriteRenderer>().color = new Color(191 / 255f, 191 / 255f, 191 / 255f, 255 / 255f);
                Hair.GetComponent<SpriteRenderer>().color = new Color(191 / 255f, 191 / 255f, 191 / 255f, 255 / 255f);

                Base.GetComponent<Animator>().SetTrigger("isHurt");
                Hair.GetComponent<Animator>().SetTrigger("isHurt");
            }
        }

        StartCoroutine("InvincibleOff");
        StartCoroutine("EndHurtAnim");
    }

    private void Die()
    {
        isDead = true;
        
        pm.GetComponent<Rigidbody2D>().mass = 1000000;
        pm.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        Base.GetComponent<Animator>().SetTrigger("isDead");
        Hair.GetComponent<Animator>().SetTrigger("isDead");

        GameManager.instance.resetStageInfo();

        StartCoroutine("DestroyPlayer");
    }

    public void RollingIncible()
    {
        isInvincible = true;
        StartCoroutine("InvincibleOff");
    }

    void CanOpenDoor()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            goDirection = Vector3.down;
        else if(Input.GetKeyDown(KeyCode.UpArrow))
            goDirection = Vector3.up;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            goDirection = Vector3.right;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            goDirection = Vector3.left;

        Debug.DrawRay(rb.position, goDirection, new Color(1, 0, 0));
        RaycastHit2D hit = Physics2D.Raycast(rb.position, goDirection, 1, LayerMask.GetMask("Door"));
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            hit.collider.GetComponent<NextWaveDoor>().DoorOpen();
        }
    }

    IEnumerator InvincibleOff()
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