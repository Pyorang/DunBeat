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

    public int currentPlayerHealth;     
    public int currentStageIndex;       //���� �������� �ѹ�
    public int currentEnemyCount;       //���� ���̺꿡�� ó���ؾ� �� ���� ��
    public int currentWaveNum;          //���� ���̺� �ѹ�
    public bool goNextWave;

    public stageInfo[] Stages;
    public NextWaveDoor nextDoor;

    void Start()
    {
        goNextWave = true;
        currentStageIndex = 0;
        currentEnemyCount = 0;
        currentWaveNum = 0;
    }

    public void nextWave()
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

    public void nextStage()
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
                    goNextWave = false;
                }
            }
        }
    }

    public void resetStageInfo()
    {
        currentStageIndex = 0;
        currentEnemyCount = 0;
        currentWaveNum = 0;
    }
}
