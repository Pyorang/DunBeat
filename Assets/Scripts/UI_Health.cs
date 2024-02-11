using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Health : MonoBehaviour
{
    [SerializeField]
    private Image[] HealthImage;

    void Start()
    {
        ChangeHealthUI();
    }

    public void ChangeHealthUI()
    {
        for (int j = 0; j < GameManager.instance.currentPlayerHealth; j++)
        {
            HealthImage[j].color = new Color(255f/255f, 255f / 255f, 255f / 255f, 255f / 255f);
        }

        for (int j = GameManager.instance.currentPlayerHealth; j < HealthImage.Length; j++)
        {
            HealthImage[j].color = new Color(65f/255f, 65f / 255f, 65f / 255f, 65f / 255f);
        }
    }
}
