using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DebugControl))]
public class DebugControlInspector :  Editor {

    private SerializedProperty m_isOn, m_isPlay, m_prop, m_cursor, m_event;

    private void OnEnable()
    {
        m_isOn = serializedObject.FindProperty("isOn");
        m_isPlay = serializedObject.FindProperty("isPlay");
        m_prop = serializedObject.FindProperty("m_Script");
        m_cursor = serializedObject.FindProperty("cursor");
        m_event = serializedObject.FindProperty("eventSystem");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUI.enabled = false;
        EditorGUILayout.PropertyField(m_prop);
        GUI.enabled = true;
        EditorGUILayout.PropertyField(m_cursor);
        EditorGUILayout.PropertyField(m_event);
        EditorGUILayout.PropertyField(m_isOn);
        if (m_isOn.boolValue)
            EditorGUILayout.PropertyField(m_isPlay);

        serializedObject.ApplyModifiedProperties();
        //base.OnInspectorGUI();
    }
}
