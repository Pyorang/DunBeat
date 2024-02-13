using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteEffect : MonoBehaviour
{
    private float timingBonus=0;

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Text effectText;

    public void NoteHitEffectSuccess(int x)
    {
        if (x == 0)
        {
            timingBonus = 2;
            effectText.text = "Perfect";
            effectText.color = new Color(0f / 255f, 255f / 255f, 208f / 255f);
        }
        else if (x == 1)
        {
            timingBonus = 1.5f;
            effectText.text = "Great";
            effectText.color = new Color(83f / 255f, 255f / 255f, 0f / 255f);
        }
        else if (x == 2)
        {
            timingBonus = 1;
            effectText.text = "Good";
            effectText.color = new Color(227f / 255f, 0f / 255f, 255f / 255f);
        }
        else if (x == 3)
        {
            timingBonus = 0.5f;
            effectText.text = "Bad";
            effectText.color = new Color(255f / 255f, 20f / 255f, 0f / 255f);
        }

        anim.SetTrigger("Hit");
    }

    public void NoteHitEffectFail()
    {
        effectText.text = "Miss";
        effectText.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        anim.SetTrigger("Hit");
    }

    public float GetTimingBonus()
    {
        return timingBonus;
    }
}
