using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;

public class AssetInfo : EveryWindow<AssetInfo>
{

    [MenuItem("Assets/AssetInfo")]
	public static void Init()
    {
        Init("AssetInfo");
    }

    private void OnGUI()
    {
        Object obj = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(obj);
        string guid = AssetDatabase.AssetPathToGUID(path);
        string localID = GetLoaclID(obj);

        SelectableField("GUID", guid);
        SelectableField("fileID", localID);
 
    }
}
