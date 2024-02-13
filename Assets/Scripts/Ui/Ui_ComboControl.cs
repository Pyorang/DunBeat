using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_ComboControl : MonoBehaviour
{
    [SerializeField]
    private GameObject comboNumText;
    [SerializeField]
    private GameObject comboText;

    private Animator anim;

    public int currentComboNum;

    void Start()
    {
        currentComboNum = 0;
        anim = GetComponent<Animator>();
    }

    void ShowComboInfo()
    {
        comboNumText.SetActive(true);
        comboText.SetActive(true);
    }

    void HideComboInfo()
    {
        comboNumText.SetActive(false);
        comboText.SetActive(false);
    }

    public void ComboSuccess()
    {
        currentComboNum++;
        comboNumText.GetComponent<Text>().text = currentComboNum.ToString();

        ShowComboInfo();

        anim.SetTrigger("comboSuccess");

        StopCoroutine("HideCombo");
        StartCoroutine("HideCombo");
    }
    
    public void ComboFail()
    {
        currentComboNum = 0;
        HideComboInfo();
    }

    IEnumerator HideCombo()
    {
        yield return new WaitForSeconds(2);
        HideComboInfo();
    }
}
