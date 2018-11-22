using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugIt {

    static string firstScene = "Assets/Scenes/LogoScene.unity";

    static string prefab = "Assets/luckyweithings/Simulator/DebugObject.prefab";

    [MenuItem("luckywei/BuildAndInstall")]
    public static void Build()
    {
        List<string> list = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                list.Add(scene.path);
            }
        }
        string[] levels = list.ToArray();
        string location = "apk/" + PlayerSettings.productName + ".apk";
        BuildTarget bt = BuildTarget.Android;
        BuildOptions bo = BuildOptions.None;
        bo |= BuildOptions.Development;
        BuildPipeline.BuildPlayer(levels, location, bt, bo);

        AdbExecute.Install();
    }

    [MenuItem("luckywei/Play")]
    static void PlayIt()
    {
        Scene s=EditorSceneManager.GetActiveScene();
        EditorSceneManager.SaveScene(s);
        if (s.path != firstScene)
        {
            s = EditorSceneManager.OpenScene(firstScene);
        }

        GameObject go = null;
        GameObject[] gos = s.GetRootGameObjects();

        bool exist = false;
        foreach (GameObject tempgo in gos)
        {
            if (tempgo.name.Contains("DebugObject"))
            {
                exist = true;
                go = tempgo;
                break;
            }
        }

        if (!exist)
        {
            GameObject obj = AssetDatabase.LoadAssetAtPath(prefab, typeof(GameObject)) as GameObject;
            go = GameObject.Instantiate(obj);
        }

        DebugControl dc = go.GetComponent<DebugControl>();
        dc.isOn = true;
        dc.isPlay = true;


        EditorApplication.isPlaying = true;
    }

    [MenuItem("luckywei/Record")]
    static void RecordIt()
    {
        

        Scene s = EditorSceneManager.GetActiveScene();
        EditorSceneManager.SaveScene(s);

        if (s.path != firstScene)
        {
            s = EditorSceneManager.OpenScene(firstScene);
        }

        GameObject go = null;

        GameObject[] gos = s.GetRootGameObjects();

        bool exist = false;
        foreach(GameObject tempgo in gos)
        {
            if(tempgo.name.Contains("DebugObject"))
            {
                exist = true;
                go = tempgo;
                break;
            }
        }

        if(!exist)
        {
            GameObject obj = AssetDatabase.LoadAssetAtPath(prefab, typeof(GameObject)) as GameObject;
            go = GameObject.Instantiate(obj);
        }

        DebugControl dc = go.GetComponent<DebugControl>();
        dc.isOn = true;
        dc.isPlay = false;


        EditorApplication.isPlaying = true;
    }
}
