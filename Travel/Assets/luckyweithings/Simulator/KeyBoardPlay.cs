using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class KeyBoardPlay : MonoBehaviour {

#if UNITY_EDITOR

    private StreamReader sr;
    private int type;
    private float time;
    private string key;
    private string line;
    private UnityEditor.EditorWindow gameView;

    private bool once = true;

    // Use this for initialization
    void Start () {

        Debug.Log("lucky start play keyboard");
        sr = FileManager.GetStreamReader((FileManager.GetFilePath("KeyBoard.rec")));

        if (sr == null)
            return;

        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        gameView = UnityEditor.EditorWindow.GetWindow(T, true, "Game") as UnityEditor.EditorWindow;

        if ((line = sr.ReadLine()) != null)
        {
            string[] ss = line.Split(' ');
            time = Convert.ToSingle(ss[0]);
            key = ss[1];
            type = Convert.ToInt32(ss[2]);
        }

    }
	
	// Update is called once per frame
	void OnGUI () {
        if (sr == null)
            return;

        if (Time.realtimeSinceStartup >= time && once)
        {
            UnityEngine.Event e = UnityEngine.Event.KeyboardEvent(key);

            e.type = (EventType)type;
            string message = time + " " + e.keyCode + " " + e.type;
            Debug.Log("lucky send key " + message);
            gameView.SendEvent(e);

            if ((line = sr.ReadLine()) != null)
            {
                string[] ss = line.Split(' ');
                time = Convert.ToSingle(ss[0]);
                key = ss[1];
                type = Convert.ToInt32(ss[2]);
            }
            else
            {
                once = false;
                Debug.Log("lucky high 播放完毕");
            }
        }
    }

#endif
}
