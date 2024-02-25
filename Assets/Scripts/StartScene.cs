using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startscene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.instance.ResetStageInfo();
            GameManager.instance.ResetPlayerStatus();
            SoundManager.instance.ResettMusicTimeNote();
            AudioSource[] audioSource = SoundManager.instance.GetComponents<AudioSource>();
            for (int i = 0; i < audioSource.Length; i++)
            {
                audioSource[i].pitch = 1f;
            }
            SceneManager.LoadScene("Stage1");
            GameManager.instance.NextStage();
        }
    }
}
