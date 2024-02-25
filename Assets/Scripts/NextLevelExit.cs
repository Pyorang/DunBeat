using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelExit : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.Space))
        {
            if(GameManager.instance.currentStageIndex == GameManager.instance.Stages.Length - 1)
            {
                SceneManager.LoadScene("EndScene");
            }
            else
            {
                string nextStage = "Stage" + (GameManager.instance.currentStageIndex + 1);
                Debug.Log(nextStage);
                GameManager.instance.NextStage();
                SceneManager.LoadScene(nextStage);
            }
        }
    }
}
