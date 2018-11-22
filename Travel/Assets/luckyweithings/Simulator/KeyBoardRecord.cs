using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class KeyBoardRecord : MonoBehaviour {

    private List<KeyCode> detected = new List<KeyCode>();
    private StreamWriter sw;

    // Use this for initialization
    void Start () {

        sw = FileManager.GetStreamWriter((FileManager.GetFilePath("KeyBoard.rec")));
        Debug.Log("luckyhigh KeyBoard Record start");
        
    }

#if UNITY_EDITOR
    // Update is called once per frame
    void OnGUI () {

        if (sw == null)
            return;

        if (UnityEngine.Event.GetEventCount()>0)
        {
            UnityEngine.Event e = UnityEngine.Event.current;
            if(e.isKey)
            {
                if(e.keyCode!=KeyCode.None)
                {
                    WriteMessage(e);
                }
            }
        }
    }
#endif

    private void WriteMessage(UnityEngine.Event e)
    {

        float time = Time.realtimeSinceStartup;
        string message = time + " " + e.keyCode + " " + (int)e.type;
        Debug.Log("lucky get key " + message);
        sw.WriteLine(message);
        sw.Flush();
    }
}
