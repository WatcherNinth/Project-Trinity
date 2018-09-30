using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System;

public class EveryWindow<T> : EditorWindow, IWindow where T : EditorWindow, IWindow{

    protected static T window;
    private static PropertyInfo inspectorMode = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

    public static void Init(string title)
    {
        window = EditorWindow.GetWindow(typeof(T), false, title) as T;
        window.ShowUp();
    }

    public static void SelectableField(string title, string value)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(title);
        EditorGUILayout.SelectableLabel(value);
        EditorGUILayout.EndHorizontal();
    }

    public void ShowUp()
    {
        DoInit();
        window.Show();
    }

    protected virtual void DoInit()
    {
    }


    public static string GetLoaclID(UnityEngine.Object obj)
    {
        if (obj == null)
            return "";
        SerializedObject so = new SerializedObject(obj);
        inspectorMode.SetValue(so, InspectorMode.Debug, null);
        SerializedProperty localId = so.FindProperty("m_LocalIdentfierInFile");
        string localID = localId.longValue.ToString();
        return localID;
    }

    public static string GetLoaclID(string path)
    {
        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
        return GetLoaclID(obj);
    }
}
