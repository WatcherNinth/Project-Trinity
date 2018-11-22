using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager
{

    public static string GetFilePath(string file)
    {
        string RecordDic = "";

#if UNITY_EDITOR
    RecordDic = Application.dataPath + "/../Record/";
        if (!Directory.Exists(RecordDic))
        {
            Directory.CreateDirectory(RecordDic);
        }
        RecordDic += file;

#elif UNITY_ANDROID
        RecordDic = "/sdcard/RecordHaha/";
        if (!Directory.Exists(RecordDic))
        {
            Directory.CreateDirectory(RecordDic);
        }
        RecordDic += file;

#elif UNITY_IPHONE
        RecordDic = Application.persistentDataPath + "/Record/";
        if (!Directory.Exists(RecordDic))
        {
            Directory.CreateDirectory(RecordDic);
        }
        RecordDic += file;
#endif

        return RecordDic;
    }

    public static FileStream GetFileReadStream(string path)
    {
        FileStream fs = null;
        try
        {
            fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            return fs;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            fs.Close();
            return null;
        }

    }

    public static FileStream GetFileWriteStream(string path)
    {
        FileStream fs = null;
        try
        {
            if (File.Exists(path))
            {
                fs = new FileStream(path, FileMode.Truncate, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            }
            return fs;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            fs.Close();
            return null;
        }
    }

    public static StreamReader GetStreamReader(string path)
    {
        StreamReader sr = null;
        try
        {
            sr = new StreamReader(path);
            return sr;
        }
        catch(Exception ex)
        {
            Debug.Log(ex.ToString());
            return null;
        }
    }

    public static StreamWriter GetStreamWriter(string path)
    {
        StreamWriter sw = null;
        try
        {
            sw = new StreamWriter(path);
            return sw;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            return null;
        }
    }

    
}
