using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FX : MonoBehaviour
{
    public static FX instance;

    [SerializeField] private UI_ResetAnim resetAnim;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {

    }

    /// <summary>
    /// 平滑淡入一个图像
    /// </summary>
    /// <typeparam name="T">Image,Sr,Tmp</typeparam>
    /// <param name="_object"></param>
    public void SmoothColorAppear<T>(T _object)
    {
        StartCoroutine(SmoothTrans<T>(_object, 1));
    }

    /// <summary>
    /// 平滑淡出一个图像
    /// </summary>
    /// <typeparam name="T">Image,Sr,Tmp</typeparam>
    /// <param name="_object"></param>
    public void SmoothColorDisappear<T>(T _object)
    {
        StartCoroutine(SmoothTrans<T>(_object, -1));
    }

    private IEnumerator SmoothTrans<T>(T _object, int _appearOrDisappear)
    {
        while (true)
        {
            if (_object is TextMeshProUGUI tmpUI)
            {
                tmpUI.color = new Color(tmpUI.color.r, tmpUI.color.g, tmpUI.color.b, tmpUI.color.a + Time.deltaTime * _appearOrDisappear);
                if ((tmpUI.color.a > 1 && _appearOrDisappear > 0) || (tmpUI.color.a < 0 && _appearOrDisappear < 0))
                    break;
            }
            else if (_object is TextMeshPro tmp)
            {
                tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a + Time.deltaTime * _appearOrDisappear);
                if ((tmp.color.a > 1 && _appearOrDisappear > 0) || (tmp.color.a < 0 && _appearOrDisappear < 0))
                    break;
            }
            else if (_object is Image img)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a + Time.deltaTime * _appearOrDisappear);
                if ((img.color.a > 1 && _appearOrDisappear > 0) || (img.color.a < 0 && _appearOrDisappear < 0))
                    break;
            }
            else if (_object is SpriteRenderer sr)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + Time.deltaTime * _appearOrDisappear);
                if ((sr.color.a > 1 && _appearOrDisappear > 0) || (sr.color.a < 0 && _appearOrDisappear < 0))
                    break;
            }
            yield return null;
        }
    }

    /// <summary>
    /// 平滑放大生成一个GO
    /// </summary>
    /// <param name="_gameObject"></param>
    /// <param name="_position"></param>
    /// <returns></returns>
    public GameObject SmoothSizeInstantiate(GameObject _gameObject, Vector3 _position)
    {
        GameObject myObject = Instantiate(_gameObject, _position, Quaternion.identity);
        StartCoroutine(SmoothSize(myObject, 1, true));
        return myObject;
    }

    /// <summary>
    /// 平滑放大出现一个已有GO
    /// </summary>
    /// <param name="_gameObject"></param>
    /// <param name="_position"></param>
    /// <returns></returns>
    public void SmoothSizeAppear(GameObject _gameObject)
    {
        StartCoroutine(SmoothSize(_gameObject, 1,false));
    }

    /// <summary>
    /// 平滑缩小消失一个已有GO
    /// </summary>
    /// <param name="_gameObject"></param>
    /// <param name="_position"></param>
    /// <returns></returns>
    public void SmoothSizeDisappear(GameObject _gameObject)
    {
        StartCoroutine(SmoothSize(_gameObject, 0, false));
    }
    private IEnumerator SmoothSize(GameObject _gameObject, int _appearOrDisappear,bool _isGenerated)
    {
        if (_gameObject.GetComponent<Image>() != null&&_appearOrDisappear == 1)
            GameController.instance.isReadyAnimating = true;

        _gameObject.SetActive(true);

        if (_gameObject!=null)
        {
            Vector3 originScale = _gameObject.transform.localScale;
            if (!_isGenerated)
            {
                originScale = Vector3.one;
            }
            if(_appearOrDisappear == 1)
            {
                _gameObject.transform.localScale = Vector3.zero;
            }
            else
            {
                _gameObject.transform.localScale = originScale;
            }
            while (true)
            {
                _gameObject.transform.localScale = Vector3.Lerp(_gameObject.transform.localScale, originScale * _appearOrDisappear, Time.unscaledDeltaTime * 5);
                if (_gameObject.transform.localScale.x > .99f && _appearOrDisappear == 1)
                {
                    _gameObject.transform.localScale = originScale;
                    break;
                }
                if (_gameObject.transform.localScale.x < .01f && _appearOrDisappear == 0)
                {
                    _gameObject.transform.localScale = Vector3.zero;
                    _gameObject.SetActive(false);
                    break;
                }
                yield return null;
            }
        }

        if (_gameObject.GetComponent<Image>() != null && _appearOrDisappear == 1)
            GameController.instance.isReadyAnimating = false;
    }

    /// <summary>
    /// 平滑刷新界面
    /// </summary>
    public void SmoothRefresh(Color _color,float _waitTime, Action _callBack = null)
    {
        StartCoroutine(SmoothRefresh_Anim(_color, _waitTime,_callBack));
    }
    private IEnumerator SmoothRefresh_Anim(Color _color,float _waitTime,Action _callBack = null)
    {
        GameController.instance.isResetAnimating = true;
        resetAnim.gameObject.SetActive(true);
        resetAnim.GetComponent<Image>().color = _color;
        resetAnim.anim.SetBool("isStart", true);
        yield return new WaitForSecondsRealtime(_waitTime);
        _callBack?.Invoke();
        GameController.instance.isResetAnimating = false;
        resetAnim.anim.SetBool("isStart", false);
    }
}
