using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{

    static public SoundManager instance;
    #region singleton
    void Awake()    //��ü ������ ���� ����
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

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;

    public string[] playSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    [SerializeField]
    private MusicTimeNote currentMusicTimeNotes;
    private int currentMusicIndex;
    public MusicTimeNote[] musicTimeNotes;

    void Strat()
    {
        currentMusicIndex = 0;
        currentMusicTimeNotes = musicTimeNotes[currentMusicIndex];
        playSoundName = new string[audioSourceEffects.Length];
    }

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("��� ���� AudioSource�� ������Դϴ�");
                return;
            }
        }
        Debug.Log(_name + "���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�.");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }

        audioSourceBgm.Stop();
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                break;
            }
        }
        Debug.Log("��� ����" + _name + "���尡 �����ϴ�.");
    }

    public void PlayBGM(string _name)
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if (_name == bgmSounds[i].name)
            {
                audioSourceBgm.clip = bgmSounds[i].clip;
                audioSourceBgm.Play();
                return;
            }
        }
        Debug.Log(_name + "���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�.");
    }

    public void NextMusicTimeNote()
    {
        currentMusicIndex++;
        currentMusicTimeNotes = musicTimeNotes[currentMusicIndex];
    }

    public void ResettMusicTimeNote()
    {
        currentMusicIndex = 0;
        currentMusicTimeNotes = musicTimeNotes[currentMusicIndex];
    }

    public MusicTimeNote GetMusicTimeNote()
    {
        return currentMusicTimeNotes;
    }

    public int GetMusicNum()
    {
        return currentMusicIndex;
    }
}
