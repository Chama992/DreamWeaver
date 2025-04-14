using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    PlayerEntityController player => GetComponentInParent<PlayerEntityController>();
    /// <summary>
    /// ���ڶ�������֡���ж�
    /// </summary>
    public void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
}
