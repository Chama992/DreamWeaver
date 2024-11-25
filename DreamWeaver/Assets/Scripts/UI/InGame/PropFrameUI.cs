using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PropFrameUI : MonoBehaviour,IPointerClickHandler
{
    private int propId;
    private Image propImage;
    private TMP_Text propCountText;
    public void Initialize(PropData _propData,int _propCount)
    { 
        propImage = transform.Find("PropIcon").GetComponent<Image>();
        propCountText = transform.Find("PropCount").GetComponent<TMP_Text>();
        propId = _propData.propID;
        propImage.sprite = _propData.propIcon;
        propCountText.text = _propCount.ToString();
        gameObject.name = propId.ToString();
    }
    public void ChangePropFrame(Sprite propFrame)
    {
        propImage.sprite = propFrame;
    }
    public void ChangePropCount(int propCount)
    {
        propCountText.text = propCount.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //TODO: 写使用道具的逻辑
        }
    }
}
