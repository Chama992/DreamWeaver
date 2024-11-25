using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    Player player => GetComponentInParent<Player>();
    /// <summary>
    /// ���ڶ�������֡���ж�
    /// </summary>
    public void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
}
