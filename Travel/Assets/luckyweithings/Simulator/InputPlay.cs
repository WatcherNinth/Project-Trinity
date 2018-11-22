using UnityEngine;
using System.Collections;
using System.IO;
using System;


public class InputPlay : MonoBehaviour {

#if UNITY_EDITOR

    private StreamReader sr;
    private string line;
    private float resw;
    private float resh;
    private UnityEditor.EditorWindow gameView;
    private bool once = true;

    private float startx;
    private float time;
    private MouseType type;
    private float xpre;
    private float ypre;

    private GameObject cursor;

    private void Awake()
    {
    }

    private void Start()
    {
        // 文件读写
        sr = FileManager.GetStreamReader((FileManager.GetFilePath("Input.rec")));
        Debug.Log("lucky start play input");
        if (sr == null)
            return;

        cursor = DebugControl.instance.cursor;

        // 获得gameView窗口
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        gameView = UnityEditor.EditorWindow.GetWindow(T, true, "Game") as UnityEditor.EditorWindow;

        // 获得gameView中渲染窗口的分辨率
        var prop = gameView.GetType().GetProperty("currentGameViewSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var gvsize = prop.GetValue(gameView, new object[0] { });
        var gvSizeType = gvsize.GetType();
        resh = (int)gvSizeType.GetProperty("height", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(gvsize, new object[0] { });
        resw = (int)gvSizeType.GetProperty("width", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(gvsize, new object[0] { });

        if ((line = sr.ReadLine()) != null)
        {
            string[] ss = line.Split(' ');
            time = Convert.ToSingle(ss[0]);
            type = (MouseType)Convert.ToInt32(ss[1]);
            xpre = Convert.ToSingle(ss[2]);
            ypre = Convert.ToSingle(ss[3]);
        }
    }

    private void Update()
    {
        if (sr == null)
            return;

        if (Time.realtimeSinceStartup >= time && once)
        {
            if (type == MouseType.LeftDown)
            {
                MyInput.SetMouseButtonDown(0);
                cursor.SetActive(true);
            }
                

            if (type == MouseType.LeftUp)
            {
                MyInput.SetMouseButtonUp(0);
            }
                

            if (type == MouseType.Move)
                MyInput.ClearMouseButton();

            /*
            float realh = gameView.position.height - 17;
            float realw = gameView.position.width;

            // 期望的宽
            float expectw = realh * resw / resh;

            Debug.Log("lucky expect " + expectw + " " + realh);
            Debug.Log("lucky pre " + xpre + " " + ypre);
            Debug.Log("lucky real res " + realw + " " + realh);
            */

            MyInput.mousePosition = new Vector3(resw * xpre, resh * ypre, 0);
            if (cursor.activeSelf)
            {
                cursor.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(resw * xpre, resh * ypre, 0);
            }
                
            
            Debug.Log("lucky play mouse " + type + " " + MyInput.mousePosition);

            if ((line = sr.ReadLine()) != null)
            {
                string[] ss = line.Split(' ');
                time = Convert.ToSingle(ss[0]);
                type = (MouseType)Convert.ToInt32(ss[1]);
                xpre = Convert.ToSingle(ss[2]);
                ypre = Convert.ToSingle(ss[3]);
            }
            else
            {
                once = false;
                Debug.Log("lucky high 播放完毕");
            }

        }
    }
        

    private void OnDestroy()
    {
        if(sr!=null)
            sr.Close();
    }

#endif
}
