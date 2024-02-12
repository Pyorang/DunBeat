using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0; //1�п� �����ϴ� ��Ʈ ����
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
            currentTime -= 60d / bpm;       //���� �ս� ����
            //Time.deltaTime�� ���ϸ� �Ϻ��� 60d/bpm��ŭ�� �ƴ� ������ �����.
            //�׷��� ������ �����ϰ� 0���� �ʱ�ȭ �ϸ� �ð��� �ս��� ����
            //���ǰ� ��Ʈ ���� �ð��� ������ ����
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
