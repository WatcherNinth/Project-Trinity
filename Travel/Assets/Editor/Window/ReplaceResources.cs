using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System.IO;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System;

public class ReplaceResources : EditorWindow {

    private Font newFont=null;
    private static PropertyInfo inspectorMode = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);
    private string dir = "Assets/";

    [MenuItem("luckywei/ReplaceFont")]
    public static void Init()
    {
        EditorSceneManager.SaveOpenScenes();
        ReplaceResources windows = (ReplaceResources)EditorWindow.GetWindow(typeof(ReplaceResources), false, "replace", true);
        windows.Show();
    }

    private void OnGUI()
    {
        newFont = (Font)EditorGUILayout.ObjectField("新字体", newFont, typeof(Font),false);
        if(GUILayout.Button("开始替换"))
        {
            Replace();
        }
    }

    private void Replace()
    {
        if(newFont!=null)
        {
            string path = AssetDatabase.GetAssetPath(newFont);
            string guid = AssetDatabase.AssetPathToGUID(path);
            Debug.Log("guid " + guid);

            SerializedObject so = new SerializedObject(newFont);
            inspectorMode.SetValue(so, InspectorMode.Debug, null);
            SerializedProperty localId = so.FindProperty("m_LocalIdentfierInFile");
            Debug.Log("local id " + localId.longValue);


            //FindPrefab();
            FindScene();
            
        }
        else
        {
            EditorUtility.DisplayDialog("error","选择的字体为空","确定");
        }
        Close();
    }

    private void FindPrefab()
    {
        int index = 0;
        string[] fis = Directory.GetFiles(dir, "*.prefab", SearchOption.AllDirectories);
        //string[] fis = Directory.GetFiles(dir, "BuyTickets.prefab", SearchOption.AllDirectories);
        foreach (string oldfi in fis)
        {
            string fi=oldfi.Replace("\\", "/");
            GameObject obj = AssetDatabase.LoadAssetAtPath(fi, typeof(GameObject)) as GameObject;

            /*
            GameObject go = Instantiate(obj);

            Text[] texts = go.GetComponentsInChildren<Text>();

            foreach (Text t in texts)
            {
                t.font = newFont;
            }
            */

            

            
            GameObject go = Instantiate(obj);
            Text[] texts = go.GetComponentsInChildren<Text>();
            
            foreach (Text t in texts)
            {
                t.font = newFont;
            }

            try
            {
                PrefabUtility.ReplacePrefab(go, obj, ReplacePrefabOptions.ConnectToPrefab);
            }
            catch(Exception e)
            {
                Debug.Log(e.ToString());
            }
            DestroyImmediate(go);
            

            string fileName = Path.GetFileName(fi);
            bool cancel=EditorUtility.DisplayCancelableProgressBar("字体替换", fileName, (float)index / (float)fis.Length);
            if (cancel)
                EditorUtility.ClearProgressBar();
            index++;
        }
        EditorUtility.ClearProgressBar();
    }

    private void FindScene()
    {
        int index = 0;
        string sceneNow=EditorSceneManager.GetActiveScene().name;
        string nowPath="";

        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            Scene s = EditorSceneManager.OpenScene(scene.path);

            foreach (GameObject root in s.GetRootGameObjects())
            {
                Text[] texts = root.GetComponentsInChildren<Text>();

                foreach (Text t in texts)
                {
                    t.font = newFont;
                }
            }
            if (s.name == sceneNow)
            {
                nowPath = scene.path;
            }
            EditorSceneManager.SaveScene(s);
            string fileName = Path.GetFileName(scene.path);
            bool cancel = EditorUtility.DisplayCancelableProgressBar("字体替换", fileName, (float)index / (float)EditorBuildSettings.scenes.Length);
            if (cancel)
                EditorUtility.ClearProgressBar();
            
        }
        EditorUtility.ClearProgressBar();
        if(! string.IsNullOrEmpty(nowPath))
            EditorSceneManager.OpenScene(nowPath);

    }


}
