using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PropFrameUI : MonoBehaviour
{
    private Image propImage;
    private TMP_Text propCountText;

    public void Initialize(PropData _propData,int _propCount)
    {
        propImage.sprite = _propData.propIcon;
        propCountText.text = _propCount.ToString();
    }

    public void ChangePropFrame(Sprite propFrame)
    {
        propImage.sprite = propFrame;
    }

    public void ChangePropCount(int propCount)
    {
        propCountText.text = propCount.ToString();
    }
}
