using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private float startPlayerAttackRange;
    [SerializeField]
    private float startPlayerMoveSpeed;

    public int currentPlayerHealth;
    public float currentPlayerAp;
    public float currentTimingBonus;
    public float playerMoveSpeed;
    public float playerAttackRange;
    public int currentStageIndex;       //현재 스테이지 넘버
    public int currentEnemyCount;       //현재 웨이브에서 처리해야 할 몬스터 수
    public int currentWaveNum;          //현재 웨이브 넘버
    public bool goNextWave;
    public Vector3 playerPosition;
    public string skillViewText;

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
                    AudioSource[] audioSource = SoundManager.instance.GetComponents<AudioSource>();
                    for (int i = 0; i < audioSource.Length; i++)
                    {
                        audioSource[i].pitch = 1f;
                    }
                    SoundManager.instance.StopAllSE();
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
        skillViewText = "";
        playerAttackRange = startPlayerAttackRange;
        playerMoveSpeed = startPlayerMoveSpeed;
    }
}
