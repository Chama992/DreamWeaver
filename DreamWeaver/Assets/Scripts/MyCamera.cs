using System.Collections;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    private Camera myCamera;

    [Tooltip("��ʼ���ͷ��С")]
    [SerializeField] private float maxCameraSize;

    [Tooltip("��С��ͷ��С")]
    [SerializeField] private float minCameraSize;

    [Tooltip("���������")]
    [SerializeField] private float mouseSensibility;

    [Tooltip("������������")]
    [SerializeField] private float mouseScrollSensibility;

    [Tooltip("����ƶ�����")]
    [SerializeField] private float maxDeltaMove;

    [Tooltip("���z����(����)")]
    [SerializeField] private float z = -10;

    private Vector3 playerPosition;

    /// <summary>
    /// ��ͼ��Χ
    /// </summary>
    private Vector3 mapRange;

    /// <summary>
    /// �Ƿ�����ɫ����
    /// </summary>
    private bool isPlayerFollowOpen = true;

    /// <summary>
    /// ǿ�ƽ�ɫ������
    /// </summary>
    private bool isPlayerFollowing;


    private void Start()
    {
        myCamera = GetComponent<Camera>();
        myCamera.orthographicSize = minCameraSize;
        GameController.instance.onLevelReady += RefreshAndShowMapRange;
    }

    private void OnDisable()
    {
        GameController.instance.onLevelReady -= RefreshAndShowMapRange;
    }
    Vector3 cameraStartPosition = Vector3.zero, mouseStartPosition = Vector3.zero;
    private void Update()
    {
        if (GameController.instance.isPausing)
        {
            return;
        }
        Vector3 playerViewPos = myCamera.WorldToViewportPoint(GameController.instance.player.transform.position);
        if (GameController.instance.isAnimating)
        {
            if (playerViewPos.x < .05 || playerViewPos.y < .05 || playerViewPos.x > .95 || playerViewPos.y > .95)
            {
                myCamera.orthographicSize += Time.deltaTime*10;
                maxCameraSize = myCamera.orthographicSize;
            }
            return;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isPlayerFollowOpen)
                isPlayerFollowOpen = false;
            else
                isPlayerFollowOpen = true;
        }
        if (isPlayerFollowOpen && !GameController.instance.isAnimating)
        {
            isPlayerFollowing = true;
        }
        else
        {
            isPlayerFollowing = false;
        }
        if (playerViewPos.x < .1 || playerViewPos.y < .1 || playerViewPos.x > .9 || playerViewPos.y > .9)
        {
            isPlayerFollowing = true;
        }
        if ( -(GameController.instance.player.transform.position - GameController.instance.levelCenterPoint).y > mapRange.y * 1.2)
        {
            //�����������
            GameController.instance.ResetLevel();
        }

        playerPosition = GameController.instance.player.transform.position + new Vector3(0, 0, z);

        if (Input.mouseScrollDelta != Vector2.zero)
        {
            myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize + Input.mouseScrollDelta.y * mouseScrollSensibility, minCameraSize, maxCameraSize);
        }

        if (Input.GetMouseButtonDown(1))
        {
            mouseStartPosition = Input.mousePosition;
            cameraStartPosition = transform.position;
        }
        if (Input.GetMouseButton(1))
        {
            transform.position = cameraStartPosition - mouseSensibility * 0.01f * (Input.mousePosition - mouseStartPosition);
        }
        else if (isPlayerFollowing)
        {
            transform.position = Vector3.Lerp(transform.position, playerPosition, maxDeltaMove * Time.deltaTime);
        }


    }

    private void RefreshAndShowMapRange()
    {
        mapRange = GameController.instance.levelEndPoint - GameController.instance.levelCenterPoint + new Vector3(GameController.instance.blockHorizontalInterval, GameController.instance.blockVerticalInterval);
        StartCoroutine(ShowMap());
        StartCoroutine(GameController.instance.GenenrateMap_Smooth());
    }

    private IEnumerator ShowMap()
    {
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, GameController.instance.levelCenterPoint + new Vector3(0, 0, z), maxDeltaMove * Time.deltaTime * 2);
            if ((transform.position - (GameController.instance.levelCenterPoint + new Vector3(0, 0, z))).magnitude < 10)
            {
                break;
            }
            yield return null;
        }
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, GameController.instance.levelCenterPoint + new Vector3(0, 0, z), maxDeltaMove * Time.deltaTime * 2);
            if (!GameController.instance.isAnimating) 
                break;
            yield return null;
        }
    }
}
