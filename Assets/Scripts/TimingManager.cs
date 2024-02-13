using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();

    [SerializeField]
    Transform center;
    [SerializeField]
    RectTransform[] timingRect;
    [SerializeField]
    private NoteEffect theEffect;

    Vector2[] timingBoxs = null;

    void Start()
    {
        theEffect = GetComponent<NoteEffect>();

        timingBoxs = new Vector2[timingRect.Length];

        for(int i =0; i < timingRect.Length; i++)
        {
            timingBoxs[i].Set(center.localPosition.x - timingRect[i].rect.width / 2, 
                              center.localPosition.x + timingRect[i].rect.width / 2);

        }
    }

    public int CheckTiming()
    {
        for(int i=0; i<boxNoteList.Count; i++)
        {
            float t_notePosX = boxNoteList[i].transform.localPosition.x;

            for (int x = 0; x < timingBoxs.Length; x++)
            {
                if (timingBoxs[x].x<=t_notePosX && t_notePosX <= timingBoxs[x].y)
                {
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    theEffect.NoteHitEffectSuccess(x);
                    boxNoteList.RemoveAt(i);
                    return x;
                }
            }
        }

        theEffect.NoteHitEffectFail();
        return timingBoxs.Length;
    }

    public int GetBoxsLength()
    {
        return timingBoxs.Length;
    }
}
