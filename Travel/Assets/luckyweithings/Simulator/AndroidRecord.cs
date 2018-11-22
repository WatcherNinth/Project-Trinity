using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AndroidRecord : MonoBehaviour {

    private StreamWriter sw;
    private bool once = true;

    // Use this for initialization
    void Start () {

        sw = FileManager.GetStreamWriter((FileManager.GetFilePath("Phone.rec")));

        if (sw == null)
            return;

    }

    private void OnDestroy()
    {

        if (sw != null)
            sw.Close();
    }

    private void OnApplicationPause(bool pause)
    {
        if(once)
        {
            once = false;
            return;
        }

        float time = Time.realtimeSinceStartup;
        if (pause)
        {
            Debug.Log("lucky app pause");
            sw.WriteLine(time + " 1");
        }
        else
        {
            Debug.Log("lucky go back");
            sw.WriteLine(time + " 0");
        }
        
    }
}
