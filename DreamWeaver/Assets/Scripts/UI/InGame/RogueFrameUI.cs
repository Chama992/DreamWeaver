using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class RogueFrameUI : MonoBehaviour,IPointerClickHandler
{
    public int propId{ get; private set; }
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
            if (PropDataManager.Instance.GetPropTypeEffectorType(propId) == PropEffectorType.Special)
            {
                GameController.instance.AddBonue(1);
                GameController.instance.AddBlackHole(1);
                GameController.instance.AddScoreModifier(1);
                InGameUIManager.Instance.CloseRoguePropPanel();
                return;
            }
            FindObjectOfType<Player>().Props.GetProps(propId,1);
            InGameUIManager.Instance.CloseRoguePropPanel();
        }
    }
}
