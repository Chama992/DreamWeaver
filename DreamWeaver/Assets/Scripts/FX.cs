using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FX : MonoBehaviour
{
    public static FX instance;

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

    public void SmoothAppear<T>(T _object)
    {
        StartCoroutine(SmoothTrans<T>(_object, 1));
    }
    public void SmoothDisappear<T>(T _object)
    {
        StartCoroutine(SmoothTrans<T>(_object, -1));
    }

    private IEnumerator SmoothTrans<T>(T _object,int _appearOrDisappear)
    {
        while (true)
        {
            if (_object is TextMeshProUGUI tmp)
            {
                tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a + Time.deltaTime * _appearOrDisappear);
                if ((tmp.color.a > 1 && _appearOrDisappear>0)||(tmp.color.a < 0 &&_appearOrDisappear<0))
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
}
