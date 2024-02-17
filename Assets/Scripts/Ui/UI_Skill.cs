using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class SelectSkillInfo
{
    public string skillInfoName;
    public string skillInfoDescription;
}

public class UI_Skill : MonoBehaviour
{

    [SerializeField]
    private Text[] gameSkillName;   //���ӿ� �� Ư�� �̸� �ؽ�Ʈ��

    [SerializeField]
    private Text[] gameSkillDescription;    //���ӿ� �� Ư�� �����

    [SerializeField]
    private SelectSkillInfo[] skillInfo;    //���ӿ� ���� ��ų Ư�� ������ �Է� ���� ��

    [SerializeField]
    private GameObject[] skillInfo_UI;      //���ӿ� ������ Ư�� �ý��� â

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private PlayerMove playerMove;

    [SerializeField]
    private UI_PauseMenu pauseMenu;

    void Start()
    {
        ShowSkillInfo();
    }

    //Ư�� �����ִ� �Լ�
    void Shuffle()
    {
        List<int> randList = new List<int>();

        for(int i = 0; i< gameSkillName.Length; i++)
        {
            while (true)
            {
                int j = Random.Range(0, skillInfo.Length);
                if (!randList.Contains(j))
                {
                    randList.Add(j);
                    break;
                }
            }
        }

        for (int i = 0; i < gameSkillName.Length; i++)
        {
            gameSkillName[i].text = skillInfo[randList[i]].skillInfoName;
            gameSkillDescription[i].text = skillInfo[randList[i]].skillInfoDescription;
        }
    }

    public void ShowSkillInfo()
    {
        Time.timeScale = 0f;
        
        for(int i = 0; i<gameSkillName.Length; i++)
        {
            skillInfo_UI[i].SetActive(true);
        }

        Shuffle();
    }

    public void HideSkillInfo()
    {
        Time.timeScale = 1f;

        for (int i = 0; i < gameSkillName.Length; i++)
        {
            skillInfo_UI[i].SetActive(false);
        }
    }

    public void SKillButtonSelected()
    {
        HideSkillInfo();

        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        string selectedSkillName = clickObject.transform.GetChild(0).GetComponent<Text>().text;

        pauseMenu.skillNames.Add(selectedSkillName);

        switch (selectedSkillName)
        {
            case "ü�� ����":
                Skill0();
                break;
            case "���� ����":
                SKill1();
                break;
            case "�츣�޽��� �Ź�":
                Skill2();
                break;
            case "���� ��":
                Skill3();
                break;
            case "�ð� �ְ�":
                Skill4();
                break;
        };
    }

    void Skill0()
    {
        playerStats.Heal();
        playerStats.Heal();
    }
    
    void SKill1()
    {
        playerStats.playerAP += 5;
    }

    void Skill2()
    {
        playerMove.moveSpeed *= 1.2f;
    }

    void Skill3()
    {
        playerMove.attackRange *= 1.2f;
    }

    void Skill4()
    {
        Time.timeScale = 0.8f;
        AudioSource[] audioSource = SoundManager.instance.GetComponents<AudioSource>();
        for(int i = 0; i < audioSource.Length; i++)
        {
            audioSource[i].pitch = 0.8f;
        }
    }
}
