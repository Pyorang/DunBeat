using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_PauseMenu : MonoBehaviour
{
    private float saveCurTimeScale;
    private bool pauseMenuOn;

    [SerializeField]
    private GameObject pauseMenuScreen;

    [SerializeField]
    private GameObject controlGameSetting;

    [SerializeField]
    private GameObject skillInfoLookScreen;

    [SerializeField]
    private Slider volumeSettingController;

    [SerializeField]
    private Text skillInfoLookScreenText;

    void Start()
    {
        pauseMenuOn = false;
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(pauseMenuOn)
                HidePauseMenu();
            else
                ShowPauseMenu();
        }
        if(pauseMenuOn)
            ControlVolume();
    }

    public void ShowPauseMenu()
    {
        saveCurTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        AudioSource[] audioSource = SoundManager.instance.GetComponents<AudioSource>();
        for (int i = 0; i < audioSource.Length; i++)
        {
            audioSource[i].pitch = 0f;
        }
        pauseMenuScreen.SetActive(true);
        pauseMenuOn = true;
    }

    void HidePauseMenu()
    {
        Time.timeScale = saveCurTimeScale;
        AudioSource[] audioSource = SoundManager.instance.GetComponents<AudioSource>();
        for (int i = 0; i < audioSource.Length; i++)
        {
            audioSource[i].pitch = saveCurTimeScale;
        }
        pauseMenuScreen.SetActive(false);
        pauseMenuOn = false;
    }
    
    public void ClickGameSettingButton()
    {
        controlGameSetting.SetActive(true);
        skillInfoLookScreen.SetActive(false);
    }

    public void ClickSkillViewButton()
    {
        controlGameSetting.SetActive(false);
        skillInfoLookScreen.SetActive(true);
        SetSkillList();
    }

    void ControlVolume()
    {
        AudioSource[] audioSource = SoundManager.instance.GetComponents<AudioSource>();
        for (int i = 0; i < audioSource.Length; i++)
        {
            audioSource[i].volume = volumeSettingController.value;
        }
    }

    public void ClickGoToMain()
    {
        SceneManager.LoadScene("StartScene");
        GameManager.instance.ResetStageInfo();
        Time.timeScale = 1f;
        AudioSource[] audioSource = SoundManager.instance.GetComponents<AudioSource>();
        for (int i = 0; i < audioSource.Length; i++)
        {
            audioSource[i].pitch = 1f;
        }
        SoundManager.instance.StopAllSE();
    }

    public void ClickExitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }

    void SetSkillList()
    {
        skillInfoLookScreenText.text = GameManager.instance.skillViewText;
    }
}
