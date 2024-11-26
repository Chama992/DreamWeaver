using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    [Tooltip("���������")]
    [SerializeField] private float mouseSensibility;

    [Tooltip("����ƶ�����")]
    [SerializeField] private float maxDeltaMove;

    [Tooltip("���z����(����)")]
    [SerializeField] private float z=-10;

    private Vector3 playerPosition;

    /// <summary>
    /// Ŀ���ƶ��ص�
    /// </summary>
    private Vector3 target;

    /// <summary>
    /// ��ͼ��Χ
    /// </summary>
    private Vector3 mapRange;

    /// <summary>
    /// �Ƿ�����ɫ����
    /// </summary>
    private bool isPlayerFollowed;

    private void Start()
    {
        GameController.instance.onLevelComplete += RefreshMapRange;
    }

    private void OnDisable()
    {
        GameController.instance.onLevelComplete -= RefreshMapRange;
    }
    Vector3 cameraStartPosition = Vector3.zero, mouseStartPosition = Vector3.zero;
    private void Update()
    {
        if(GameController.instance.isPausing)
        {
            return;
        }

        playerPosition = GameController.instance.player.transform.position + new Vector3(0,0,z);
        if(Input.GetMouseButtonDown(1))
        {
            mouseStartPosition = Input.mousePosition;
            cameraStartPosition = transform.position;
        }

        if(Input.GetMouseButton(1))
        {
            transform.position = cameraStartPosition - (Input.mousePosition - mouseStartPosition)*mouseSensibility*0.01f;
        }
        else if (Mathf.Abs(target.x - GameController.instance.levelCenterPoint.x) > Mathf.Abs(mapRange.x) || Mathf.Abs(transform.position.y - GameController.instance.levelCenterPoint.y) > Mathf.Abs(mapRange.y))
        {
            target = GameController.instance.levelCenterPoint + new Vector3(0,0,z);
            transform.position = Vector3.Lerp(transform.position, target, maxDeltaMove * 0.01f);
        }
        else if(isPlayerFollowed)
        {
            transform.position = Vector3.Lerp(transform.position, playerPosition, maxDeltaMove * 0.01f);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(isPlayerFollowed)
                isPlayerFollowed = false;
            else
                isPlayerFollowed = true;
        }
    }

    private void RefreshMapRange()
    {
        mapRange = GameController.instance.levelEndPoint - GameController.instance.levelCenterPoint + new Vector3(GameController.instance.blockHorizontalInterval,GameController.instance.blockVerticalInterval);
        StartCoroutine(DetectPosition());
    }

    private IEnumerator DetectPosition()
    {
        while(true)
        {
            isPlayerFollowed = true;
            if((transform.position-GameController.instance.player.transform.position).magnitude<1)
            {
                GameController.instance.CameraMoved();
                break;
            }
            yield return null;
        }
    }
}
