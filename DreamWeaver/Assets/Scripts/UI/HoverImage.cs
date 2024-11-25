using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverImage : MonoBehaviour
{
    public GameObject hoverImage;
    public Button button;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        // ���ó�ʼ״̬�²���ʾ hoverImage
        hoverImage.SetActive(false);

        //��ȡbutton�Բ�ͬUI�¼�����Ӧ���
        button.gameObject.AddComponent<EventTrigger>();
        EventTrigger trigger = button.GetComponent<EventTrigger>();

        //������button��Χ�¼�
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();//����һ���¼������ڴ�� ��������һ�¼�
        entryEnter.eventID = EventTriggerType.PointerEnter;

        entryEnter.callback.AddListener((eventData) => { OnHoverEnter(); });
        trigger.triggers.Add(entryEnter);

        //����˳�button��Χ�¼�
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;

        entryExit.callback.AddListener((eventData) => { OnHoverExit(); });
        trigger.triggers.Add(entryExit);
    }
    void OnHoverEnter()//��ͣ��ʾ,���ı���ɫ
    {
        hoverImage.SetActive(true);
        text.color = Color.white; 
    }

    void OnHoverExit()// �Ƴ����أ��Ļ��ı���ɫ
    {
        hoverImage.SetActive(false);
        text.color = Color.black; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
