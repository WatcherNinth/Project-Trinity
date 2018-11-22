using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class KeyWordFind : EveryWindow<KeyWordFind> {

    class ScriptPosition
    {
        public string path;
        public int line;
        public string lineword;

        public ScriptPosition(string p, int l, string w)
        {
            path = p;
            line = l;
            lineword = w;
        }
    }

    private string keyword;
    private string[] ss;
    private List<ScriptPosition> results = new List<ScriptPosition>();
    private Vector2 scrollPosition = Vector2.zero;

    [MenuItem("luckywei/Find KeyWord")]
    public static void Init()
    {
        Init("Find KeyWord");
    }

    public void Find()
    {
        string[] files = Directory.GetFiles("Assets/", "*.cs", SearchOption.AllDirectories);
        int index = 1;
        foreach (string file in files)
        {

            CheckFile(file);

            index++;
            string name = Path.GetFileName(file);
            bool isCancel = EditorUtility.DisplayCancelableProgressBar("查找代码", name, (float)index / files.Length);
            if (isCancel)
            {
                EditorUtility.ClearProgressBar();
                break;
            }
        }
        EditorUtility.ClearProgressBar();
    }

    private void CheckFile(string file)
    {

        StreamReader sr = new StreamReader(file);

        string line = "";
        int n = 0;

        bool oneline = false;
        if (ss.Length == 1)
        {
            oneline = true;
        }

        List<string> total = new List<string>();
        while ((line = sr.ReadLine()) != null)
        {
            if(oneline)
            {
                n++;
                if(line.Contains(keyword))
                {
                    ScriptPosition sp = new ScriptPosition(file, n, line);
                    results.Add(sp);
                }
            }
            else
            {
                total.Add(line);
            }

        }

        if (oneline)
            return;

        while (n < total.Count)
        {
            line = total[n];
            if (LineEquals(line, ss[0], 0))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(line + "\r\n");

                int i = 1;
                for (; i < ss.Length - 1; i++)
                {
                    line = total[n + i];

                    // 文章读完
                    if (line == null)
                        break;

                    // 中间行不匹配
                    if (!LineEquals(line, ss[i], 2))
                        break;

                    // 都通过
                    sb.Append(line + "\r\n");
                }

                // 文章读完
                if (line == null)
                    break;
                // 中间行不对，回到读完第一行的起始
                if (i != ss.Length - 1)
                {
                    n++;
                }
                else
                {
                    line = total[n + i];

                    // 没有最后一行，文章结束，跳出大循环
                    if (line == null)
                    {
                        break;
                    }
                    // 最后一行不匹配
                    else if (!LineEquals(line, ss[i], 1))
                    {
                        n++;
                    }
                    // 最后一行匹配
                    else
                    {
                        sb.AppendLine(line);
                        ScriptPosition sp = new ScriptPosition(file, n+1, sb.ToString());
                        results.Add(sp);
                        n += ss.Length;
                    }
                }

            }
            else
            {
                n++;
            }
        }
    }

    private bool LineEquals(string line, string compare, int type)
    {
        bool equals = false;
        switch(type)
        {
            case 0:
                equals = firstLineEquals(line, compare);
                break;
            case 1:
                equals = lastlineEquals(line, compare);
                break;
            default:
                equals = (line == compare);
                break;
        }
        return equals;
    }

    private bool firstLineEquals(string line, string first)
    {
        int linelen = line.Length;
        int firstlen = first.Length;
        if (linelen >= firstlen)
        {
            int start = linelen - firstlen;
            string sub = line.Substring(start);
            if (sub == first)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    private bool lastlineEquals(string line, string last)
    {
        int linelen = line.Length;
        int lastlen = last.Length;
        if (linelen >= lastlen)
        {
            int start = linelen - lastlen;
            string sub = line.Substring(0,lastlen);
            if (sub == last)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("KeyWord");
        keyword = EditorGUILayout.TextArea(keyword);

        if(GUILayout.Button("Search") && !string.IsNullOrEmpty(keyword))
        {
            results.Clear();
            string[] spilts = { "\r\n" };
            ss = keyword.Split(spilts, StringSplitOptions.None);

            Find();
        }

        EditorGUILayout.Space();
        
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Scripts");

        if (results.Count == 0)
        {
            EditorGUILayout.LabelField("no results");
            EditorGUILayout.EndVertical();
            return;
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        foreach (ScriptPosition sp in results)
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(sp.path);
            EditorGUILayout.LabelField("line: " + sp.line);
            EditorGUILayout.EndVertical();
            if (GUILayout.Button("Open"))
            {
                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(sp.path, typeof(UnityEngine.Object)) as UnityEngine.Object;
                Selection.activeObject = obj;
                AssetDatabase.OpenAsset(obj, sp.line);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.TextArea(sp.lineword);

            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}
