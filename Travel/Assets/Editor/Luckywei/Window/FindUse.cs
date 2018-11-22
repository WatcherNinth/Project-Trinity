using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class FindUse : EveryWindow<FindUse> {

    string guid = "";
    List<string> resultsPrefab = new List<string>();
    List<string> resultsScene = new List<string>();
    Vector2 scrollPositionPrefab = Vector2.zero;
    Vector2 scrollPositionScene = Vector2.zero;

    [MenuItem("Assets/Find Use")]
    public static void Init()
    {
        Init("Find Use");
    }

    protected override void DoInit()
    {
        base.DoInit();
        window.Find();
    }

    public void Find()
    {
        Object obj = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(obj);
        guid = AssetDatabase.AssetPathToGUID(path);

        Search("*.prefab", resultsPrefab);
        Search("*.unity", resultsScene);

    }

    private void Search(string things, List<string> results)
    {
        string[] files = Directory.GetFiles("Assets/", things, SearchOption.AllDirectories);
        int index = 1;
        foreach (string file in files)
        {
            string name = Path.GetFileName(file);
            StreamReader sr = new StreamReader(file);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains(guid))
                {
                    results.Add(file);
                    break;
                }
            }
            sr.Close();
            index++;
            bool isCancel = EditorUtility.DisplayCancelableProgressBar("查找引用", name, (float)index / files.Length);
            if (isCancel)
                EditorUtility.ClearProgressBar();
        }
        EditorUtility.ClearProgressBar();
    }

    private void Show(string name, List<string> result, Vector2 scrollPosition)
    {
        //EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField(name);

        if(result.Count==0)
        {
            EditorGUILayout.LabelField("no results");
            return;
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        foreach (string path in result)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(path);
            if (GUILayout.Button("Open"))
            {
                Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object)) as Object;
                Selection.activeObject = obj;
                AssetDatabase.OpenAsset(obj);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        //EditorGUILayout.EndVertical();
    }

    private void OnGUI()
    {
        SelectableField("GUID", guid);

        EditorGUILayout.Space();

        //EditorGUILayout.BeginHorizontal();
        Show("Prefab", resultsPrefab, scrollPositionPrefab);

        EditorGUILayout.Space();

        Show("Scene", resultsScene, scrollPositionScene);
       // EditorGUILayout.EndHorizontal();
    }


}
