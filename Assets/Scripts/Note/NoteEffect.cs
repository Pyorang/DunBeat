using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteEffect : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Text effectText;

    public void NoteHitEffectSuccess(int x)
    {
        if (x == 0)
            effectText.text = "Perfect";
        else if (x == 1)
            effectText.text = "Great";
        else if (x == 2)
            effectText.text = "Good";
        else if (x == 3)
            effectText.text = "Bad";

        anim.SetTrigger("Hit");
    }

    public void NoteHitEffectFail()
    {
        effectText.text = "Miss";

        anim.SetTrigger("Hit");
    }
}
