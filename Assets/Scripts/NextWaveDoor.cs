using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextWaveDoor : MonoBehaviour
{
    [SerializeField]
    private int stageNumber;
    [SerializeField]
    private int waveNumber;

    public void DoorOpen()
    {
        if(stageNumber == GameManager.instance.currentStageIndex && waveNumber == GameManager.instance.currentWaveNum)
        {
            if(GameManager.instance.goNextWave)
            {
                GameManager.instance.nextWave();
                Destroy(gameObject);
            }
        }
    }    
}
