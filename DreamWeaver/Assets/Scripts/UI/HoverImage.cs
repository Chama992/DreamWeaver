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
        // 设置初始状态下不显示 hoverImage
        hoverImage.SetActive(false);

        //获取button对不同UI事件的响应组件
        button.gameObject.AddComponent<EventTrigger>();
        EventTrigger trigger = button.GetComponent<EventTrigger>();

        //鼠标进入button范围事件
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();//定义一个事件，用于存放 鼠标进入这一事件
        entryEnter.eventID = EventTriggerType.PointerEnter;

        entryEnter.callback.AddListener((eventData) => { OnHoverEnter(); });
        trigger.triggers.Add(entryEnter);

        //鼠标退出button范围事件
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;

        entryExit.callback.AddListener((eventData) => { OnHoverExit(); });
        trigger.triggers.Add(entryExit);
    }
    void OnHoverEnter()//悬停显示,改文本颜色
    {
        hoverImage.SetActive(true);
        text.color = Color.white; 
    }

    void OnHoverExit()// 移出隐藏，改回文本颜色
    {
        hoverImage.SetActive(false);
        text.color = Color.black; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
