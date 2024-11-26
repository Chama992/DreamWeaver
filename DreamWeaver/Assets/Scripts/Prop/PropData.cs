using UnityEngine;


[CreateAssetMenu(fileName = "Prop", menuName = "Prop/Prop", order = 0)]
public class PropData : ScriptableObject
{
    public int propID;
    public string propName;
    public string propDescription;
    public Sprite propIcon;
    public PropType propType;
    public PropEffectorType propEffectorType;
}
