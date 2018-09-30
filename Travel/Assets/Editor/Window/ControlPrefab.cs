using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Threading;

public class ControlPrefab {

    //[MenuItem("luckywei/Control Prefab")]
	
    public static void Control()
    {
        GameObject obj = Selection.activeGameObject;
        if (obj == null)
            return;
        //在文件目录夹中
        if (string.IsNullOrEmpty(obj.scene.name))
        {
            
            string path=AssetDatabase.GetAssetPath(obj);
            Debug.Log(path);
        }
        //在场景中
        else
        {
            
            Debug.Log(obj.name);
            string prefabPath = "Assets/Resources/Prefabs/" + obj.name + ".prefab";
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (asset == null)
            {
                Debug.Log("create new prefab");
                //重新生成一个新GameObject，是为了不和当前的gameObject建立连接
                GameObject go = GameObject.Instantiate(obj);
                PrefabUtility.CreatePrefab(prefabPath, go, ReplacePrefabOptions.ConnectToPrefab);
                GameObject.DestroyImmediate(go);
            }
            else
            {
                Debug.Log("replace prefab");
                Object newprefab = PrefabUtility.CreateEmptyPrefab(prefabPath);
                PrefabUtility.ReplacePrefab(obj, newprefab, ReplacePrefabOptions.ConnectToPrefab);
            }
        }
    }

    [MenuItem("Assets/Find References")]
    public static void Find()
    {
        
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        string guid=AssetDatabase.AssetPathToGUID(path);
        Debug.Log(guid);
        Debug.Log(Application.dataPath);
        int startindex = 0;
        
        EditorApplication.update = delegate ()
          {
              bool isCancel = EditorUtility.DisplayCancelableProgressBar("countdown","text", (float)startindex / (float)100);
              if(isCancel || startindex >= 100)
              {
                  Debug.Log("stop");
                  EditorUtility.ClearProgressBar();
                  EditorApplication.update = null;
                  startindex = 0;
              }
              Thread.Sleep(1000);
              startindex++;
          };

        
    }
}
