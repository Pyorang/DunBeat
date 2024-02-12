using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0; //1분에 생성하는 노트 개수
    double currentTime = 0d;

    [SerializeField]
    Transform tfNoteAppear = null;
    
    //[SerializeField]
    //GameObject goNote = null;

    TimingManager theTimingManager;

    void Start()
    {
        theTimingManager = GetComponent<TimingManager>();
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= 60d / bpm)
        {
            GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
            t_note.transform.position = tfNoteAppear.position;
            t_note.SetActive(true);
            //GameObject t_note = Instantiate(goNote, tfNoteAppear.position, Quaternion.identity);
            //t_note.transform.SetParent(this.transform);
            theTimingManager.boxNoteList.Add(t_note);
            currentTime -= 60d / bpm;       //오차 손실 방지
            //Time.deltaTime을 더하면 완벽한 60d/bpm만큼이 아닌 오차가 생긴다.
            //그래서 오차를 무시하고 0으로 초기화 하면 시간적 손실이 생김
            //음악과 노트 생성 시간에 오차가 생김
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            theTimingManager.boxNoteList.Remove(collision.gameObject);

            ObjectPool.instance.noteQueue.Enqueue(collision.gameObject);
            collision.gameObject.SetActive(false);
            //Destroy(collision.gameObject);
        }
    }
}
