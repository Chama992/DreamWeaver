using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Unity.VisualScripting;

//ʹ�õ���ģʽ�������̳е������࣬ʹ���ڳ����л�ʱ������
public class BrightnessManager : PersistentSingleton<BrightnessManager>//panel��slider�İ�Manager
{
    //�ӿ�
    public Image coverPanel;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    //͸�������ã�����Slider on valuechanged�ϣ�ʵ����slider����ͼƬ͸����
    public void TransparencySldOnClick(float value)
    {
        Color currentColor = coverPanel.color;
        currentColor.a = value;
        coverPanel.color = currentColor;
        //
        // Debug.Log("��ǰSliderֵ" + value);
        // Debug.Log("��ǰ͸����" + coverPanel.color.a);
    }
}
