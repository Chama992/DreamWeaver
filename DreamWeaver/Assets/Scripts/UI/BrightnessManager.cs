using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Unity.VisualScripting;

//使用单例模式开发，继承单例父类，使其在场景切换时不销毁
public class BrightnessManager : PersistentSingleton<BrightnessManager>//panel和slider的绑定Manager
{
    //接口
    public Image coverPanel;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    //透明度设置，挂在Slider on valuechanged上，实现用slider调节图片透明度
    public void TransparencySldOnClick(float value)
    {
        Color currentColor = coverPanel.color;
        currentColor.a = value;
        coverPanel.color = currentColor;
        //
        // Debug.Log("当前Slider值" + value);
        // Debug.Log("当前透明度" + coverPanel.color.a);
    }
}
