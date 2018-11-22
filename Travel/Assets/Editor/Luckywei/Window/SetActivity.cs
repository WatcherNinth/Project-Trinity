using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SetActivity : EveryWindow<SetActivity>
{
    public static string activity = "";
    public static string apkname = "";
    public static void Init()
    {
        activity = PlayerPrefs.GetString("LuckyweiActivity");
        Init("设置参数");

    }

    private void OnGUI()
    {
        activity = EditorGUILayout.TextField("Main Activity", activity);
        EditorGUILayout.LabelField("apk 路径");
        EditorGUILayout.SelectableLabel("apk\\" + PlayerSettings.productName + ".apk");
        if(GUILayout.Button("确定"))
        {
            PlayerPrefs.SetString("LuckyweiActivity", activity);
            window.Close();
        }
    }

}
