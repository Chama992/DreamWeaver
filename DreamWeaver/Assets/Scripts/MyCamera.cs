using System.Collections;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    [Tooltip("���������")]
    [SerializeField] private float mouseSensibility;

    [Tooltip("����ƶ�����")]
    [SerializeField] private float maxDeltaMove;

    [Tooltip("���z����(����)")]
    [SerializeField] private float z = -10;

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
        GameController.instance.onGameStart += RefreshAndShowMapRange;
        GameController.instance.onLevelReady += RefreshAndShowMapRange;
    }

    private void OnDisable()
    {
        GameController.instance.onGameStart -= RefreshAndShowMapRange;
        GameController.instance.onLevelReady -= RefreshAndShowMapRange;
    }
    Vector3 cameraStartPosition = Vector3.zero, mouseStartPosition = Vector3.zero;
    private void Update()
    {
        if (GameController.instance.isPausing || GameController.instance.isCounting)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isPlayerFollowed)
                isPlayerFollowed = false;
            else
                isPlayerFollowed = true;
        }

        playerPosition = GameController.instance.player.transform.position + new Vector3(0, 0, z);
        if (Input.GetMouseButtonDown(1))
        {
            mouseStartPosition = Input.mousePosition;
            cameraStartPosition = transform.position;
        }

        if (Input.GetMouseButton(1))
        {
            transform.position = cameraStartPosition - mouseSensibility * 0.01f * (Input.mousePosition - mouseStartPosition);
        }
        else if (Mathf.Abs(transform.position.x - GameController.instance.levelCenterPoint.x) > Mathf.Abs(mapRange.x) || Mathf.Abs(transform.position.y - GameController.instance.levelCenterPoint.y) > Mathf.Abs(mapRange.y))
        {
            target = GameController.instance.levelCenterPoint + new Vector3(0, 0, z);
            transform.position = Vector3.Lerp(transform.position, target, maxDeltaMove * Time.deltaTime);
        }
        else if (isPlayerFollowed)
        {
            transform.position = Vector3.Lerp(transform.position, playerPosition, maxDeltaMove * Time.deltaTime);
        }

        //�ж������û����Ϊɶд��������Ҳ��֪����
        if(Mathf.Abs(GameController.instance.player.transform.position.x - GameController.instance.levelCenterPoint.x) > Mathf.Abs(mapRange.x) || Mathf.Abs(GameController.instance.player.transform.position.y - GameController.instance.levelCenterPoint.y) > Mathf.Abs(mapRange.y))
        {
            GameController.instance.ResetLevel();
        }
    }

    private void RefreshAndShowMapRange()
    {
        mapRange = GameController.instance.levelEndPoint - GameController.instance.levelCenterPoint + new Vector3(GameController.instance.blockHorizontalInterval*2, GameController.instance.blockVerticalInterval*2);
        StartCoroutine(ShowMap());
    }

    private IEnumerator ShowMap()
    {
        isPlayerFollowed = false;
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, GameController.instance.levelCenterPoint +new Vector3(0,0,z), maxDeltaMove * Time.deltaTime);
            if ((transform.position - (GameController.instance.levelCenterPoint + new Vector3(0,0,z))).magnitude < 1)
            {
                break;
            }
            yield return null;
        }
        float waitTime = 1f;
        while (waitTime > 0)
        {
            transform.position = Vector3.Lerp(transform.position, GameController.instance.levelCenterPoint + new Vector3(0, 0, z), maxDeltaMove * Time.deltaTime);
            waitTime -= Time.deltaTime;
            yield return null;
        }
        GameController.instance.StartLevel();
        GameController.instance.isCounting = false;
    }
}
