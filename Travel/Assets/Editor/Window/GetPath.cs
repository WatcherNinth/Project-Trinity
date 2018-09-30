using UnityEngine;
using System.Collections;
using UnityEditor;

public class GetPath : EveryWindow<GetPath>
{

    private string guid;

	[MenuItem("luckywei/Get Path")]
    public static void Init()
    {
        Init("GetPath");
    }

    private void OnGUI()
    {
        guid = EditorGUILayout.TextField("GUID", guid);

        if(!string.IsNullOrEmpty(guid))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            string localID = GetLoaclID(path);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            SelectableField("name", name);
            SelectableField("path", path);
            SelectableField("localID", localID);
            EditorGUILayout.Space();
            if (GUILayout.Button("Open"))
            {
                if (!string.IsNullOrEmpty(path))
                {
                    Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object)) as Object;
                    AssetDatabase.OpenAsset(obj);
                }
            }
        }

        
    }
}
