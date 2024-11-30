using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Help_ChangeFont : MonoBehaviour
{
    [SerializeField]private TMP_FontAsset font;
    private void OnValidate()
    {
        TextMeshProUGUI[] TMPs = FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Include,FindObjectsSortMode.None);
        for (int i = 0; i < TMPs.Length; ++i)
        {
            TMPs[i].font = font;
        }
        string[] paths;
        paths = new string[1] { "Assets/Prefabs" };
        string[] _guids = AssetDatabase.FindAssets("t:Prefab",paths);
        foreach (var _guid in _guids)
        {
            string _prefabPath = AssetDatabase.GUIDToAssetPath(_guid);
            GameObject _prefab = AssetDatabase.LoadAssetAtPath(_prefabPath, typeof(GameObject)) as GameObject;
            if (_prefab.GetComponentInChildren<TextMeshProUGUI>() != null)
            {
                _prefab.GetComponentInChildren<TextMeshProUGUI>().font = font;
            }
        }
    }
    private List<GameObject> GetAllSceneObjectsWithInactive()
    {
        var allTransforms = Resources.FindObjectsOfTypeAll(typeof(Transform));
        var previousSelection = Selection.objects;
        Selection.objects = allTransforms.Cast<Transform>()
            .Where(x => x != null)
            .Select(x => x.gameObject)
            //如果你只想获取所有在Hierarchy中被禁用的物体，反注释下面代码
            //.Where(x => x != null && !x.activeInHierarchy)
            .Cast<UnityEngine.Object>().ToArray();

        var selectedTransforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
        Selection.objects = previousSelection;

        return selectedTransforms.Select(tr => tr.gameObject).ToList();
    }
}
