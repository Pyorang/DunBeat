using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnemyStatus : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyName;
    [SerializeField]
    private GameObject enemyHealthBar;
    [SerializeField]
    private Image enemyHealthBarFill;

    public void ShowStatus()
    {
        enemyName.SetActive(true);
        enemyHealthBar.SetActive(true);
        StopCoroutine("HideStatus");
        StartCoroutine("HideStatus");
    }

    public void Hide_Status()
    {
        enemyName.SetActive(false);
        enemyHealthBar.SetActive(false);
    }

    IEnumerator HideStatus()
    {
        yield return new WaitForSeconds(2f);
        enemyName.SetActive(false);
        enemyHealthBar.SetActive(false);
    }

    public void ChangeEnemy(GameObject enemy)
    {
        Enemy _enemy = enemy.GetComponent<Enemy>();
        enemyName.GetComponent<Text>().text = _enemy.GetName();
        enemyHealthBarFill.fillAmount = _enemy.GetCurHp()/_enemy.GetMaxHp();
    }


}
