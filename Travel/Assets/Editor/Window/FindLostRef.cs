using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor.SceneManagement;

public class FindLostRef : EveryWindow<FindLostRef>
{
    private string guidfront = "guid: ";
    private string filefront = "fileID: ";

    [MenuItem("luckywei/Find Missing Reference")]
    public static void Init()
    {
        Init("Find Missing Reference");
    }

    protected override void DoInit()
    {
        base.DoInit();
        EditorSceneManager.SaveOpenScenes();
        //window.Find();
    }

    private void OnGUI()
    {
        
    }

    private void Find()
    {
        string[] files = Directory.GetFiles("Assets/","*.prefab", SearchOption.AllDirectories);
        int index = 0;
        foreach(string file in files)
        {
            string path = file.Replace("\\", "/");
            string guid = AssetDatabase.AssetPathToGUID(path);
            string localID = GetLoaclID(path);

            /*
            Debug.Log("localID " + localID);
            Debug.Log("guid " + guid);
            Debug.Log("path " + path);
            */
            Debug.Log("now prefab " + path);
            SearchPrefab(guid, localID, file, files);
            SearchScene(guid, localID);

            string name = Path.GetFileNameWithoutExtension(path);
            bool isCancel=EditorUtility.DisplayCancelableProgressBar("修复错误引用", name, (float)index / files.Length);
            if(isCancel || index >= files.Length)
            {
                EditorUtility.ClearProgressBar();
            }
            
        }
        EditorUtility.ClearProgressBar();
    }

    public void SearchPrefab(string guid, string localID, string prefabpath, string[] files)
    {
        
        foreach (string file in files)
        {
            if (file == prefabpath)
                continue;
            StreamReader sr = new StreamReader(file);
            string line = "";
            StringBuilder sb = new StringBuilder();
            bool write = false;
            while ((line = sr.ReadLine()) != null)
            {
                string content = guidfront + guid;
                if (line.Contains(content))
                {

                    int start = line.IndexOf(":") + 1;
                    start = line.IndexOf(":", start) + 1;
                    int end = line.IndexOf(",", 0) - 1;
                    if (start < 0 || end < 0)
                        continue;
                    string wrongfileID = line.Substring(start + 1, end - start);
                    if (wrongfileID != localID)
                    {
                        Debug.Log("file " + file);
                        Debug.Log("wrongfile ID " + wrongfileID);
                        Debug.Log("local ID " + localID);
                        Debug.Log("line " + line);
                        string newline = line.Replace(wrongfileID, localID);
                        Debug.Log("newline " + newline);
                        sb.Append(newline);
                        write = true;
                    }
                    else
                    {
                        sb.Append(line);
                    }
                }
                else
                {
                    sb.Append(line);
                }
                sb.Append("\r\n");
            }
            sr.Close();
            if (write)
            {
                Debug.Log("fix prefab " + file);
                FileStream fs = new FileStream(file, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(sb.ToString());
                sw.Flush();
                sw.Close();
                fs.Close();

            }
        }
    }

    public void SearchScene(string guid, string localID)
    {
        
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            string file = scene.path;
            StreamReader sr = new StreamReader(file);
            string line = "";
            StringBuilder sb = new StringBuilder();
            bool write = false;
            while ((line = sr.ReadLine()) != null)
            {
                string content = guidfront + guid;
                if (line.Contains(content))
                {

                    int start = line.IndexOf(":") + 1;
                    start = line.IndexOf(":", start) + 1;
                    int end = line.IndexOf(",", 0) - 1;
                    if (start > 0 && start < end)
                    {
                        string wrongfileID = line.Substring(start + 1, end - start);
                        if (wrongfileID != localID)
                        {
                            Debug.Log("file " + file);
                            Debug.Log("wrongfile ID " + wrongfileID);
                            Debug.Log("local ID " + localID);
                            Debug.Log("line " + line);
                            string newline = line.Replace(wrongfileID, localID);
                            Debug.Log("newline " + newline);
                            sb.Append(newline);
                            write = true;
                        }
                        else
                        {
                            sb.Append(line);
                        }
                    }
                    else
                    {
                        sb.Append(line);
                    }
                }
                else
                {
                    sb.Append(line);
                }
                sb.Append("\r\n");
            }
            sr.Close();
            if (write)
            {
                Debug.Log("fix scene " + file);
                string name = Path.GetFileNameWithoutExtension(file);
                if(name=="Login")
                {
                    FileStream fs = new FileStream("E:\\"+name, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(sb.ToString());
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
                else
                {
                    FileStream fs = new FileStream(file, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(sb.ToString());
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
                

            }
        }
    }

    
}
