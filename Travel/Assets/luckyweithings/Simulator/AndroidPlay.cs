using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AndroidPlay : MonoBehaviour {

#if UNITY_EDITOR

    private StreamReader sr;

    private float sleepTime;
    private float firstTime;
    private bool once = true;

    // Use this for initialization
    void Start () {
        UnityEngine.Debug.Log("lucky start play Android");
        sr = FileManager.GetStreamReader((FileManager.GetFilePath("Phone.rec")));

        if (sr == null)
            return;

        once = ReadTwoLine();

    }
	
	// Update is called once per frame
	void Update () {

        if(firstTime <= Time.realtimeSinceStartup && once)
        {
            SetPause(sleepTime);
            once = ReadTwoLine();
        }

    }

    

    private void SetPause(float s)
    {

        Process process = new Process();
        process.StartInfo.FileName = "git-bash.exe";
        process.StartInfo.Arguments = @"Assets\Editor\Luckywei\shell\sleep.sh " + s;
        process.StartInfo.CreateNoWindow = false;
        process.StartInfo.ErrorDialog = true;
        process.StartInfo.UseShellExecute = false;

        process.Start();

    }

    private bool ReadTwoLine()
    {

        string line = "";
        float first = 0;
        while((line=sr.ReadLine())!=null)
        {
            string[] ss = line.Split(' ');
            int status = Convert.ToInt32(ss[1]);

            if(status==1)
            {
                UnityEngine.Debug.Log("s " + ss);
                first = Convert.ToSingle(ss[0]);
                break;
            }

        }



        if ((line = sr.ReadLine()) != null)
        {
            string[] ss = line.Split(' ');
            int status = Convert.ToInt32(ss[1]);

            if(status==0)
            {
                float second = Convert.ToSingle(ss[0]);
                sleepTime = second - first;
                firstTime = first;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }
#endif

}
