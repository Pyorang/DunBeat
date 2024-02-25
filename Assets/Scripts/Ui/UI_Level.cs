using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Level : MonoBehaviour
{
    [SerializeField]
    private Text levelText;
    
    void Start()
    {
        levelText.text = GameManager.instance.currentStageIndex.ToString();
    }
}
