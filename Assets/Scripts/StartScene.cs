using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.instance.ResetPlayerStatus();
            SceneManager.LoadScene("Stage1");
            GameManager.instance.NextStage();
        }
    }
}
