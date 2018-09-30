using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(ParentScrollViewRect), true)]
public class ParentScrollViewInspector : ScrollRectEditor
{

    private SerializedProperty Speed, ShowLatested, prop;

    protected override void OnEnable()
    {
        Speed = serializedObject.FindProperty("triggerspeed");
        ShowLatested = serializedObject.FindProperty("ShowLatested");
        prop = serializedObject.FindProperty("m_Script");
        base.OnEnable();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUI.enabled = false;
        EditorGUILayout.PropertyField(prop);
        GUI.enabled = true;
        EditorGUILayout.LabelField("My Own Property");
        EditorGUILayout.PropertyField(Speed);
        EditorGUILayout.PropertyField(ShowLatested);
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Original Property");
        base.OnInspectorGUI();
        
    }
}
