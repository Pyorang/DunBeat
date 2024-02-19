using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteFlame : MonoBehaviour
{
    private bool musicStart;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!musicStart)
        {
            if (collision.CompareTag("Note"))
            {
                SoundManager.instance.PlayBGM(SoundManager.instance.bgmSounds[SoundManager.instance.GetMusicNum() - 1].name);
                musicStart = true;
            }
        }
    }
}
