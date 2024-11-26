using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class RogueFrameUI : MonoBehaviour,IPointerClickHandler
{
    private int propId;
    private Image propImage;
    private TMP_Text propDescriptionText;
    private TMP_Text propName;
    public void Initialize(PropData _propData)
    { 
        propImage = transform.Find("PropIcon").GetComponent<Image>();
        propName = transform.Find("PropName").GetComponent<TMP_Text>();
        propDescriptionText = transform.Find("PropDescription").GetComponent<TMP_Text>();
        propId = _propData.propID;
        propImage.sprite = _propData.propIcon;
        propName.text = _propData.propName;
        propDescriptionText.text = _propData.propDescription;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            FindObjectOfType<Player>().Props.GetProps(propId,1);
            InGameUIManager.Instance.CloseRoguePropPanel();
        }
    }
}