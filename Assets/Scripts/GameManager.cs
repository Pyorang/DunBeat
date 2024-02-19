using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class stageInfo
{
    public int StageIndex;
    public int[] waveEnemyCount;
}

public class GameManager : MonoBehaviour
{

    static public GameManager instance;

    #region singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(this.gameObject);
    }
    #endregion singleton

    [SerializeField]
    private int startPlayerHealth;
    [SerializeField]
    private float startPlayerAp;

    public int currentPlayerHealth;
    public float currentPlayerAp;
    public float currentTimingBonus;
    public int currentStageIndex;       //���� �������� �ѹ�
    public int currentEnemyCount;       //���� ���̺꿡�� ó���ؾ� �� ���� ��
    public int currentWaveNum;          //���� ���̺� �ѹ�
    public bool goNextWave;

    public GameObject currentEnemy;
    public stageInfo[] Stages;
    public NextWaveDoor nextDoor;
    public UI_EnemyStatus enemyStaus;

    void Start()
    {
        goNextWave = true;
        currentStageIndex = 0;
        currentEnemyCount = 0;
        currentWaveNum = 0;
    }

    public void NextWave()
    {
        if (goNextWave)
        {
            if (currentEnemyCount == 0)
            {
                if (currentWaveNum != Stages[currentStageIndex].waveEnemyCount.Length - 1)
                {
                    currentWaveNum++;
                    currentEnemyCount = Stages[currentStageIndex].waveEnemyCount[currentWaveNum];
                    goNextWave = false;
                }
            }
        }
    }

    public void NextStage()
    {
        if(goNextWave)
        {
            if (currentEnemyCount == 0)
            {
                if (currentWaveNum == Stages[currentStageIndex].waveEnemyCount.Length - 1)
                {
                    currentWaveNum = 0;
                    currentStageIndex++;
                    currentEnemyCount = Stages[currentStageIndex].waveEnemyCount[currentWaveNum];
                    SoundManager.instance.NextMusicTimeNote();
                    goNextWave = false;
                }
            }
        }
    }

    public void ResetStageInfo()
    {
        goNextWave = true;
        currentStageIndex = 0;
        currentEnemyCount = 0;
        currentWaveNum = 0;
        SoundManager.instance.ResettMusicTimeNote();
    }

    public void ResetPlayerStatus()
    {
        currentPlayerHealth = startPlayerHealth;
        currentPlayerAp = startPlayerAp;
    }
}
